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
    public class EndOfLifeTreatmentAppService : AsyncCrudAppService<EndOfLifeTreatmentData, EndOfLifeTreatmentDataDto, Guid, EndOfLifeTreatmentResponseDto, CreateEndOfLifeTreatmentDto, CreateEndOfLifeTreatmentDto>, IEndOfLifeTreatmentAppService
    {
        private readonly IRepository<EndOfLifeTreatmentData, Guid> _endOfLifeTreatmentRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly ILogger<EndOfLifeTreatmentAppService> _logger;


        public EndOfLifeTreatmentAppService
            (IRepository<EndOfLifeTreatmentData, Guid> endOfLifeTreatmentRepository,
             IRepository<ActivityData, Guid> activityRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository,
             IRepository<EmissionsSource, int> emissionsSourceRepository,
             IRepository<OrganizationUnit, Guid> organizationUnitRepository,
             IRepository<EmissionGroups, Guid> emissionGroupsRepository) : base(endOfLifeTreatmentRepository)
        {
            _endOfLifeTreatmentRepository = endOfLifeTreatmentRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emissionGroupsRepository = emissionGroupsRepository;
        }

        /// <summary>
        /// Method used to add End-of-life treatment of sld products activity data as well as a correlating emission.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<EndOfLifeTreatmentData> AddEndOfLifeTreatmentDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                if (input.WasteTreatmentMethod == null) throw new UserFriendlyException("Waste treatment method cannot be null.");

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);
                var activityId = Guid.NewGuid();
                var activityModel = new EndOfLifeTreatmentData
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
                    WasteTreatmentMethod = (int)input.WasteTreatmentMethod,
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

                var result = await _endOfLifeTreatmentRepository.InsertAsync(activityModel);
                await _emissionsRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddEndOfLifeTreatmentDataAsync - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to get End-of-life treatment of sold products activity data as well as emissions data.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<List<ActivityDataVM>> GetEndOfLifeTreatmentData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var endOfLifeTreatmentData = from eol in _endOfLifeTreatmentRepository.GetAll()
                                         join a in _activityRepository.GetAll() on eol.Id equals a.Id into eola
                                         from a in eola.DefaultIfEmpty()
                                         join at in _activityTypeRepository.GetAll() on eol.ActivityTypeId equals at.Id into eolat
                                         from at in eolat.DefaultIfEmpty()
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
                                             WasteTreatmentMethod = eol.WasteTreatmentMethod,
                                             EmissionGroupId = eol.EmissionGroupId,
                                             GroupName = eg.Name
                                         };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    endOfLifeTreatmentData = endOfLifeTreatmentData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));
                }

                var result = await endOfLifeTreatmentData.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetEndOfLifeTreatmentData - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to update End-of-life treatment of sold products activity data as well as the correlating emission data. <br/>
        /// It first checks if emission data exists, if not it will add it.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<EndOfLifeTreatmentData> UpdateEndOfLifeTreatmentDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                if (input.WasteTreatmentMethod == null) throw new UserFriendlyException("Waste treatment method cannot be null");

                var activityTypeId = _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId).Id;
                var endOfLifeTreatmentData = _endOfLifeTreatmentRepository.Get((Guid)input.Id);

                endOfLifeTreatmentData.Name = input.Name;
                endOfLifeTreatmentData.Quantity = (float)input.Quantity;
                endOfLifeTreatmentData.UnitId = input.UnitId;
                endOfLifeTreatmentData.TransactionDate = (DateTime)input.ConsumptionEnd;
                endOfLifeTreatmentData.ActivityTypeId = activityTypeId;
                endOfLifeTreatmentData.Description = input.Description;
                endOfLifeTreatmentData.IsActive = true;
                endOfLifeTreatmentData.DataQualityType = DataQualityType.Actual;
                endOfLifeTreatmentData.IndustrialProcessId = 1;
                endOfLifeTreatmentData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                endOfLifeTreatmentData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                endOfLifeTreatmentData.OrganizationUnitId = input.OrganizationUnitId;
                endOfLifeTreatmentData.isProcessed = false;
                endOfLifeTreatmentData.WasteTreatmentMethod = (int)input.WasteTreatmentMethod;
                endOfLifeTreatmentData.EmissionGroupId = input.EmissionGroupId;
                endOfLifeTreatmentData.EmissionFactorId = input.EmissionFactorId;
                endOfLifeTreatmentData.Status = input.ActivityDataStatus;

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

                var result = await _endOfLifeTreatmentRepository.UpdateAsync(endOfLifeTreatmentData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateEndOfLifeTreatmentDataAsync - Exception: {ex}");
                return null;
            }
        }


    }
}