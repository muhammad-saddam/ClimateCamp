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
    public class FuelAndEnergyAppService : AsyncCrudAppService<FuelAndEnergyData, FuelAndEnergyDto, Guid, FuelAndEnergyResponseDto, CreateFuelAndEnergyDto, CreateFuelAndEnergyDto>, IFuelAndEnergyAppService
    {
        private readonly IRepository<FuelAndEnergyData, Guid> _fuelAndEnergyRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly ILogger<FuelAndEnergyAppService> _logger;

        public FuelAndEnergyAppService(
            IRepository<FuelAndEnergyData, Guid> fuelAndEnergyRepository,
            IRepository<ActivityData, Guid> activityRepository,
            IRepository<ActivityType, int> activityTypeRepository,
            IRepository<Emission, Guid> emissionsRepository,
            IRepository<EmissionsSource, int> emissionsSourceRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IRepository<EmissionGroups, Guid> emissionGroupsRepository) : base(fuelAndEnergyRepository)
        {
            _fuelAndEnergyRepository = fuelAndEnergyRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _emissionGroupsRepository = emissionGroupsRepository;
        }

        /// <summary>
        /// Add Fuel and Energy related activity data and a corresponding emission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FuelAndEnergyData> AddFuelAndEnergyDataAsync(ActivityDataVM input)
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

                var activityId = Guid.NewGuid();

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityModel = new FuelAndEnergyData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = (float)input.Quantity,
                    UnitId = input.UnitId,
                    TransactionDate = (DateTime)input.ConsumptionEnd,
                    ActivityTypeId = input.ActivityTypeId ?? activityTypes.FirstOrDefault(x => x.Name.Contains("Generic"))?.Id,
                    Description = input.Description,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    //todo: confirm?
                    IndustrialProcessId = 1,
                    ConsumptionStart = (DateTime)input.ConsumptionStart,
                    ConsumptionEnd = (DateTime)input.ConsumptionEnd,
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    EmissionGroupId = input.EmissionGroupId,
                    EmissionFactorId = input.EmissionFactorId,
                    Status = input.ActivityDataStatus,
                    FuelTypeId = input.FuelTypeId,
                    EnergyType = (EnergyType?)input.EnergyType
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

                var result = await _fuelAndEnergyRepository.InsertAsync(activityModel);
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
                _logger.LogError($"Method: AddFuelAndEnergyDataAsync - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to retrieve Fuel and Energy related ActivityData, along with correlated emissions if they exist.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<List<ActivityDataVM>> GetFuelAndEnergyData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var fuelAndEnergyData = from f in _fuelAndEnergyRepository.GetAll()
                                            join a in _activityRepository.GetAll() on f.Id equals a.Id into fa
                                            from a in fa.DefaultIfEmpty()
                                            join at in _activityTypeRepository.GetAll() on f.ActivityTypeId equals at.Id into tat
                                            from at in tat.DefaultIfEmpty()
                                            join es in _emissionsSourceRepository.GetAll() on at.EmissionsSourceId equals es.Id into ates
                                            from es in ates.DefaultIfEmpty()
                                            join ou in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals ou.Id into aou
                                            from ou in aou.DefaultIfEmpty()
                                            join e in _emissionsRepository.GetAll() on a.Id equals e.ActivityDataId into ae
                                            from e in ae.DefaultIfEmpty()
                                            join eg in _emissionGroupsRepository.GetAll() on a.EmissionGroupId equals eg.Id into aeg
                                            from eg in aeg.DefaultIfEmpty()
                                            where ou.OrganizationId == organizationId && es.Id == emissionSourceId
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
                                                GroupName = eg.Name,
                                                ActivityTypeId = a.ActivityTypeId
                                            };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    fuelAndEnergyData = fuelAndEnergyData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));
                }

                var result = await fuelAndEnergyData.ToListAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetFuelAndEnergyData - Exception: {ex}");
                return null;
            }
        }


        public async Task<FuelAndEnergyData> UpdateFuelAndEnergyDataAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityTypeId = input.ActivityTypeId ?? _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId && x.Name.Contains("Generic"))?.Id;
                var fuelAndEnergyData = _fuelAndEnergyRepository.Get((Guid)input.Id);

                fuelAndEnergyData.Name = input.Name;
                fuelAndEnergyData.Quantity = (float)input.Quantity;
                fuelAndEnergyData.UnitId = input.UnitId;
                fuelAndEnergyData.TransactionDate = (DateTime)input.ConsumptionEnd;
                fuelAndEnergyData.ActivityTypeId = activityTypeId;
                fuelAndEnergyData.Description = input.Description;
                fuelAndEnergyData.IsActive = true;
                fuelAndEnergyData.DataQualityType = DataQualityType.Actual;
                fuelAndEnergyData.IndustrialProcessId = 1;
                fuelAndEnergyData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                fuelAndEnergyData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                fuelAndEnergyData.OrganizationUnitId = input.OrganizationUnitId;
                fuelAndEnergyData.isProcessed = false;
                fuelAndEnergyData.EmissionGroupId = input.EmissionGroupId;
                fuelAndEnergyData.EmissionFactorId = input.EmissionFactorId;
                fuelAndEnergyData.Status = input.ActivityDataStatus;
                fuelAndEnergyData.FuelTypeId = input.FuelTypeId;
                fuelAndEnergyData.EnergyType = (EnergyType?)input.EnergyType;

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

                var result = await _fuelAndEnergyRepository.UpdateAsync(fuelAndEnergyData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateFuelAndEnergyDataAsync - Exception: {ex}");
                return null;
            }
        }
    }
}
