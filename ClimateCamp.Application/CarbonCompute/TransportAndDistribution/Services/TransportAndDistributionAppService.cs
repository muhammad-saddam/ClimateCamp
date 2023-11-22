using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Azure.Storage.Blobs.Models;
using ClimateCamp.Application.Common;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class TransportAndDistributionAppService : AsyncCrudAppService<TransportAndDistributionData, TransportAndDistributionDataDto, Guid, TransportAndDistributionResponseDto, CreateTransportAndDistributionDataDto, CreateTransportAndDistributionDataDto>, ITransportAndDistributionAppService
    {
        private readonly IRepository<TransportAndDistributionData, Guid> _transportAndDistributionRepository;
        private readonly IRepository<ActivityData, Guid> _activityRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<Unit, int> _unitRepository;
        private readonly IRepository<VehicleType, Guid> _vehicleTypeRepository;
        private readonly ILogger<TransportAndDistributionAppService> _logger;

        public TransportAndDistributionAppService
            (IRepository<TransportAndDistributionData, Guid> transportAndDistributionRepository,
            IRepository<ActivityData, Guid> activityRepository,
            IRepository<ActivityType, int> activityTypeRepository,
            IRepository<Emission, Guid> emissionRepository,
            IRepository<EmissionsSource, int> emissionsSourceRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IRepository<Unit, int> unitRepository,
            IRepository<VehicleType, Guid> vehicleTypeRepository) : base(transportAndDistributionRepository)
        {
            _transportAndDistributionRepository = transportAndDistributionRepository;
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionRepository = emissionRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _unitRepository = unitRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }
        /// <summary>
        /// This method is used to add Transport and Distribution activity data as well as a corresponding emission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TransportAndDistributionData> AddTransportDataAndEmissionsAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                var unitGroupId = _unitRepository.GetAll().Where(x => x.Id == input.UnitId).FirstOrDefault().Group;
                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);

                if (unitGroupId == (int)GHG.UnitGroup.Currency)
                {
                    activityTypes = activityTypes.Where(x => x.Id == (int)GHG.ActivityTypeEnum.UpstreamTransportationSpendBased);
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var emissionSourceName = _emissionsSourceRepository.Get(input.EmissionSourceId).Name;
                var transportType = emissionSourceName == "Upstream transportation" ? TransportAndDistributionType.Upstream : TransportAndDistributionType.Downstream;
                var activityId = Guid.NewGuid();
                var activityModel = new TransportAndDistributionData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = input.Quantity ?? 0,
                    UnitId = input.UnitId,
                    //TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = input.ConsumptionEnd ?? default(DateTime),
                    ActivityTypeId = activityTypes.FirstOrDefault()?.Id,
                    Description = input.Description,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = input.ConsumptionStart ?? default(DateTime) ,
                    ConsumptionEnd = input.ConsumptionEnd ?? default(DateTime) ,
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = input.VehicleTypeId ?? Guid.Empty,
                    SupplierOrganization = input.SupplierOrganization,
                    Type = (int)transportType,
                    GoodsQuantity = input.GoodsQuantity,
                    GoodsUnitId = input.GoodsUnitId,
                    Distance = input.Distance,
                    DistanceUnitId = input.DistanceUnitId,
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

                var result = await _transportAndDistributionRepository.InsertAsync(activityModel);
                await _emissionRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddTransportDataAndEmissionsAsync - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to retrieve TransportAndDistribution ActivityData and correlating Emissions. <br/>
        /// If there is no emission entry associated with the activity data entry, it will still return the activity data but with default values. <br/>
        /// It is used to populate the table view in the Upstream/Downstream categories.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <returns></returns>
        public async Task<List<TransportationDataVM>> GetTransportationData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd)
        {
            try
            {
                var transportData = from t in _transportAndDistributionRepository.GetAll()
                                    join a in _activityRepository.GetAll() on t.Id equals a.Id into ta
                                    from a in ta.DefaultIfEmpty()
                                    join at in _activityTypeRepository.GetAll() on t.ActivityTypeId equals at.Id into tat
                                    from at in tat.DefaultIfEmpty()
                                    join es in _emissionsSourceRepository.GetAll() on at.EmissionsSourceId equals es.Id into ates
                                    from es in ates.DefaultIfEmpty()
                                    join ou in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals ou.Id into aou
                                    from ou in aou.DefaultIfEmpty()
                                    join e in _emissionRepository.GetAll() on a.Id equals e.ActivityDataId into ae
                                    from e in ae.DefaultIfEmpty()
                                    where ou.OrganizationId == organizationId
                                        && es.Id == emissionSourceId
                                    select new TransportationDataVM
                                    {
                                        Id = a.Id,
                                        CO2e = e.CO2E,
                                        CO2eUnitId = e.CO2EUnitId,
                                        DataQualityType = (int)a.DataQualityType,
                                        Quantity = a.Quantity,
                                        QuantityUnitId = a.UnitId,
                                        OrganizationUnitName = ou.Name,
                                        Name = a.Name,
                                        ConsumptionStart = a.ConsumptionStart,
                                        ConsumptionEnd = a.ConsumptionEnd,
                                        SupplierOrganization = t.SupplierOrganization
                                    };

                if (consumptionStart != null && consumptionEnd != null)
                {
                    transportData = transportData.Where(x => (x.ConsumptionEnd >= consumptionStart.Value.Date && x.ConsumptionStart <= consumptionEnd.Value.Date));

                }

                var result = await transportData.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetTransportationData - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Method used to update the existing data for a TransportAndDistribution activity data and the correlating emission entry.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TransportAndDistributionData> UpdateTransportDataAndEmissionsAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                if (input.CO2e != null && input.CO2eUnitId == null)
                {
                    throw new UserFriendlyException("The Unit for CO2e cannot be null.", "If there is a value for CO2e, then there must be a value for CO2eUnitId as well.");
                }

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var unitGroupId = _unitRepository.GetAll().Where(x => x.Id == input.UnitId).FirstOrDefault().Group;
                var activityTypes = _activityTypeRepository.GetAll().Where(x => x.EmissionsSourceId == input.EmissionSourceId);
               
                if (unitGroupId == (int)GHG.UnitGroup.Currency)
                {
                    activityTypes = activityTypes.Where(x => x.Id == (int)GHG.ActivityTypeEnum.UpstreamTransportationSpendBased);
                }

                var transportAndDistributionData = _transportAndDistributionRepository.Get((Guid)input.Id);
                var emissionSourceName = _emissionsSourceRepository.Get(input.EmissionSourceId).Name;
                var transportType = emissionSourceName == "Upstream transportation" ? TransportAndDistributionType.Upstream : TransportAndDistributionType.Downstream;


                transportAndDistributionData.OrganizationUnitId = input.OrganizationUnitId;
                transportAndDistributionData.Name = input.Name;
                transportAndDistributionData.Description = input.Description;
                transportAndDistributionData.ActivityTypeId = activityTypes.FirstOrDefault()?.Id; ;
                transportAndDistributionData.ConsumptionStart = (DateTime)input.ConsumptionStart;
                transportAndDistributionData.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                transportAndDistributionData.TransactionDate = (DateTime)input.ConsumptionEnd;
                transportAndDistributionData.Quantity = (float)input.Quantity;
                transportAndDistributionData.UnitId = input.UnitId;
                transportAndDistributionData.VehicleTypeId = (Guid)input.VehicleTypeId;
                transportAndDistributionData.SupplierOrganization = input.SupplierOrganization;
                transportAndDistributionData.Type = (int)transportType;
                transportAndDistributionData.GoodsQuantity = input.GoodsQuantity;
                transportAndDistributionData.GoodsUnitId = input.GoodsUnitId;
                transportAndDistributionData.Distance = input.Distance;
                transportAndDistributionData.DistanceUnitId = input.DistanceUnitId;
                transportAndDistributionData.EmissionGroupId = input.EmissionGroupId;
                transportAndDistributionData.EmissionFactorId = input.EmissionFactorId;
                transportAndDistributionData.Status = input.ActivityDataStatus;

                var emissionRow = _emissionRepository
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

                    await _emissionRepository.InsertAsync(emissionsModel);
                }

                var result = await _transportAndDistributionRepository.UpdateAsync(transportAndDistributionData);
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message, userEx.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateTransportDataAndEmissionsAsync - Exception: {ex}");
                return null;
            }
        }


        public async Task<PagedResultDto<VehicleTypesDto>> GetAllVehiclesByModeOfTransport(int modeOfTransport)
        {
            var vehicles = await _vehicleTypeRepository.GetAll()
                .Where(x => x.ModeOfTransport == modeOfTransport)
                .OrderBy(x => x.Name)
                .Select(x => new VehicleTypesDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ModeOfTransport = x.ModeOfTransport
                    }).ToListAsync();

            var result = new PagedResultDto<VehicleTypesDto>()
            {
                Items = vehicles,
                TotalCount = vehicles.Count
            };

            return result;
        }

    }
}
