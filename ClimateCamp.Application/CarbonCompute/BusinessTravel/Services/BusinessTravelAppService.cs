using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ClimateCamp.Application.Common;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;


namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class BusinessTravelAppService : AsyncCrudAppService<BusinessTravelData, BusinessTravelDataDto, Guid, BusinessTravelResponseDto, CreateBusinessTravelDataDto, CreateBusinessTravelDataDto>, IBusinessTravelAppService
    {
        private readonly IRepository<BusinessTravelData, Guid> _businessTravelRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly ILogger<BusinessTravelAppService> _logger;

        public BusinessTravelAppService
            (IRepository<BusinessTravelData, Guid> businessTravelRepository,
             IRepository<ActivityData, Guid> activityRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository,
             IRepository<EmissionsSource, int> emissionsSourceRepository,
             IRepository<OrganizationUnit, Guid> organizationUnitRepository,
             IRepository<EmissionGroups, Guid> emissionGroupsRepository) : base(businessTravelRepository)
        {
            _businessTravelRepository = businessTravelRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emissionGroupsRepository = emissionGroupsRepository;
        }

        /// <summary>
        /// Add business travel activity data as well as a corresponding emission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BusinessTravelData> AddBusinessTravelDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");
                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }
                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);
                var emissionSourceName = _emissionsSourceRepository.Get(input.EmissionSourceId).Name;

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityId = Guid.NewGuid();
                var activityModel = new BusinessTravelData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = (float)input.Quantity,
                    UnitId = input.UnitId,
                    //TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = (DateTime)input.ConsumptionEnd,
                    ActivityTypeId = activityTypes.FirstOrDefault()?.Id,
                    Description = input.Description,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = (DateTime)input.ConsumptionStart,
                    ConsumptionEnd = (DateTime)input.ConsumptionEnd,
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = (Guid)input.VehicleTypeId,
                    EmissionGroupId = input.EmissionGroupId,
                    EmissionFactorId = input.EmissionFactorId,
                    Status = input.ActivityDataStatus
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = input.OrganizationUnitId,
                    EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = calculatedEmissions,
                    CO2EUnitId = input.CO2eUnitId,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                var result = await _businessTravelRepository.InsertAsync(activityModel);
                await _emissionsRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddBusinessTravelDataAsync - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to retrieve business travel ActivityData, along with correlated emissions if they exist.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<List<ActivityDataVM>> GetBusinessTravelData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var businessTravelData = from bt in _businessTravelRepository.GetAll()
                                          join a in _activityRepository.GetAll() on bt.Id equals a.Id into bta
                                          from a in bta.DefaultIfEmpty()
                                          join at in _activityTypeRepository.GetAll() on bt.ActivityTypeId equals at.Id into btat
                                          from at in btat.DefaultIfEmpty()
                                          join es in _emissionsSourceRepository.GetAll() on at.EmissionsSourceId equals es.Id into ates
                                          from es in ates.DefaultIfEmpty()
                                          join ou in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals ou.Id into aou
                                          from ou in aou.DefaultIfEmpty()
                                          join e in _emissionsRepository.GetAll() on a.Id equals e.ActivityDataId into ae
                                          from e in ae.DefaultIfEmpty()
                                          join eg in _emissionGroupsRepository.GetAll() on a.EmissionGroupId equals eg.Id into aeg
                                          from eg in aeg.DefaultIfEmpty()
                                         where ou.OrganizationId == organizationId
                                              && es.Id == emissionSourceId
                                          select new ActivityDataVM
                                          {
                                              Id = a.Id,
                                              Name = a.Name,
                                              OrganizationUnit = ou.Name,
                                              OrganizationUnitId = ou.Id,
                                              EmissionSourceId = es.Id,
                                              IsProcessed = a.isProcessed,
                                              IsDeleted = a.IsDeleted,
                                              Quantity = a.Quantity,
                                              UnitId = (int)a.UnitId,
                                              Description = a.Description,
                                              ConsumptionStart = a.ConsumptionStart,
                                              ConsumptionEnd = a.ConsumptionEnd,
                                              TransactionDate = a.TransactionDate,
                                              CO2e = e.CO2E,
                                              CO2eUnitId = e.CO2EUnitId,
                                              VehicleTypeId = bt.VehicleTypeId,
                                              VehicleTypeName = bt.VehicleType.Name,
                                              EmissionGroupId = bt.EmissionGroupId,
                                              GroupName = eg.Name
                                          };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    businessTravelData = businessTravelData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));
                }

                var result = await businessTravelData.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetBusinessTravelData - Exception: {ex}");
                return null;
            }
        }

        public async Task<BusinessTravelData> UpdateBusinessTravelDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityTypeId = _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId).Id;
                var businessTravelData = _businessTravelRepository.Get((Guid)input.Id);

                businessTravelData.Name = input.Name;
                businessTravelData.Quantity = (float)input.Quantity;
                businessTravelData.UnitId = input.UnitId;
                businessTravelData.TransactionDate = (DateTime)input.ConsumptionEnd;
                businessTravelData.ActivityTypeId = activityTypeId;
                businessTravelData.Description = input.Description;
                businessTravelData.IsActive = true;
                businessTravelData.DataQualityType = DataQualityType.Actual;
                businessTravelData.IndustrialProcessId = 1;
                businessTravelData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                businessTravelData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                businessTravelData.OrganizationUnitId = input.OrganizationUnitId;
                businessTravelData.isProcessed = false;
                businessTravelData.VehicleTypeId = (Guid)input.VehicleTypeId;
                businessTravelData.EmissionGroupId = input.EmissionGroupId;
                businessTravelData.EmissionFactorId = input.EmissionFactorId;
                businessTravelData.Status = input.ActivityDataStatus;

                var emissionRow = _emissionsRepository
                    .GetAll()
                    .Where(e => e.ActivityDataId == input.Id)
                    .FirstOrDefault();

                if (emissionRow != null)
                {
                    emissionRow.OrganizationUnitId = input.OrganizationUnitId;
                    emissionRow.CO2E = calculatedEmissions;
                    emissionRow.CO2EUnitId = input.CO2eUnitId;
                }
                else
                {
                    var emissionsModel = new Emission
                    {
                        OrganizationUnitId = input.OrganizationUnitId,
                        EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                        EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                        ResponsibleEntityID = Guid.Empty,
                        ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                        IsActive = true,
                        CO2E = calculatedEmissions,
                        CO2EUnitId = input.CO2eUnitId,
                        CreationTime = DateTime.UtcNow,
                        ActivityDataId = (Guid)input.Id
                    };

                    await _emissionsRepository.InsertAsync(emissionsModel);
                }

                var result = await _businessTravelRepository.UpdateAsync(businessTravelData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateBusinessTravelDataAsync - Exception: {ex}");
                return null;
            }
        }

    }
}
