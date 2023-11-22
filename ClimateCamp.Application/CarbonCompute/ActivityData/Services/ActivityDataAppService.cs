using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using ClimateCamp.Application.CarbonCompute;
using ClimateCamp.Application.Common;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using ClimateCamp.Core.Notifications;
using FileHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    /// <summary>
    /// 
    /// </summary>
    public class ActivityDataAppService : AsyncCrudAppService<ActivityData, ActivityDataDto, Guid, ActivityDataResponseDto, CreateActivityDataDto, CreateActivityDataDto>
    {
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<StationaryCombustionData, Guid> _stationaryCombustionDataRepository;
        private readonly IRepository<PurchasedEnergyData, Guid> _purchasedEnergyDataRepository;
        private readonly IRepository<MobileCombustionData, Guid> _mobileCombustionDataRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly IRepository<Core.Organization, Guid> _organizationRepository;
        private readonly IRepository<Unit, int> _unitRepository;
        private readonly IRepository<ClimateCamp.CarbonCompute.ActivityType, int> _activityTypeRepository;
        private readonly IRepository<EmissionsSource, int> _emissionsSourceRepository;
        private readonly IRepository<FuelType, Guid> _fuelTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<EmissionGroups, Guid> _emissionGroupsRepository;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<ActivityDataAppService> _logger;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<EmissionsSource, int> _emissionSourceRepository;
        private readonly IRepository<PurchasedProductsData, Guid> _purchasedProductDataRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<BusinessTravelData, Guid> _businessTravelDataRepository;
        private readonly IRepository<EmployeeCommuteData, Guid> _employeeCommuteDataRepository;
        private readonly IRepository<TransportAndDistributionData, Guid> _transportAndDistributionDataRepository;
        private readonly IRepository<FugitiveEmissionsData, Guid> _fugitiveEmissionsDataRepository;
        private readonly IRepository<GreenhouseGas, Guid> _greenhouseGasesRepository;
        private readonly IRepository<WasteGeneratedData, Guid> _wasteGeneratedDataRepository;
        private readonly IRepository<EndOfLifeTreatmentData, Guid> _endOfLifeTreatmentDataRepository;
        private readonly IRepository<EmissionsFactor, Guid> _emissionFactorRepository;
        private readonly IRepository<EmissionsFactorsLibrary, Guid> _emissionFactorsLibraryRepository;
        private readonly IRepository<Core.ConversionFactors, Guid> _conversionFactorsRepository;
        private readonly IRepository<ProductEmissions, Guid> _productEmissionsRepository;
        private readonly IRepository<CustomerProduct, Guid> _customerProductRepository;
        private readonly IRepository<Core.EmissionsSummary, Guid> _emissionsSummaryRepository;

        private List<Guid> emissionGroupIdList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityDataRepository"></param>
        /// <param name="unitRepository"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="organizationRepository"></param>
        /// <param name="activityTypeRepository"></param>
        /// <param name="clientFactory"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="fuelTypeRepository"></param>
        /// <param name="stationaryCombustionDataRepository"></param>
        /// <param name="mobileCombustionDataRepository"></param>
        /// <param name="emissionsSourceRepository"></param>
        /// <param name="emissionsRepository"></param>
        /// <param name="emissionGroupsRepository"></param>
        /// <param name="notificationPublisher"></param>
        /// <param name="purchasedEnergyDataRepository"></param>
        /// <param name="emissionSourceRepository"></param>
        /// <param name="purchasedProductDataRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="businessTravelDataRepository"></param>
        /// <param name="employeeCommuteDataRepository"></param>
        /// <param name="transportAndDistributionDataRepository"></param>
        /// <param name="fugitiveEmissionsDataRepository"></param>
        /// <param name="greenhouseGasesRepository"></param>
        /// <param name="wasteGeneratedDataRepository"></param>
        /// <param name="endOfLifeTreatmentDataRepository"></param>
        /// <param name="emissionFactorRepository"></param>
        /// <param name="emissionFactorsLibraryRepository"></param>
        /// <param name="conversionFactorsRepository"></param>
        /// <param name="productEmissionsRepository"></param>
        /// <param name="customerProductRepository"></param>
        /// <param name="emissionsSummaryRepository"></param>
        public ActivityDataAppService(
            IRepository<ActivityData, Guid> activityDataRepository,
            IRepository<Unit, int> unitRepository,
            IRepository<OrganizationUnit, Guid> organizationUnitRepository,
            IRepository<Core.Organization, Guid> organizationRepository,
            IRepository<ClimateCamp.CarbonCompute.ActivityType, int> activityTypeRepository,
            IHttpClientFactory clientFactory, IConfiguration config,
            ILogger<ActivityDataAppService> logger,
            IRepository<FuelType, Guid> fuelTypeRepository,
            IRepository<PurchasedEnergyData, Guid> purchasedEnergyDataRepository,
            IRepository<StationaryCombustionData, Guid> stationaryCombustionDataRepository,
            IRepository<MobileCombustionData, Guid> mobileCombustionDataRepository,
            IRepository<EmissionsSource, int> emissionsSourceRepository,
            IRepository<Emission, Guid> emissionsRepository,
            IRepository<EmissionGroups, Guid> emissionGroupsRepository,
            INotificationPublisher notificationPublisher,
            IRepository<EmissionsSource, int> emissionSourceRepository,
            IRepository<PurchasedProductsData, Guid> purchasedProductDataRepository,
            IRepository<Product, Guid> productRepository,
            IRepository<BusinessTravelData, Guid> businessTravelDataRepository,
            IRepository<EmployeeCommuteData, Guid> employeeCommuteDataRepository,
            IRepository<TransportAndDistributionData, Guid> transportAndDistributionDataRepository,
            IRepository<FugitiveEmissionsData, Guid> fugitiveEmissionsDataRepository,
            IRepository<GreenhouseGas, Guid> greenhouseGasesRepository,
            IRepository<WasteGeneratedData, Guid> wasteGeneratedDataRepository,
            IRepository<EndOfLifeTreatmentData, Guid> endOfLifeTreatmentDataRepository,
            IRepository<EmissionsFactor, Guid> emissionFactorRepository,
            IRepository<EmissionsFactorsLibrary, Guid> emissionFactorsLibraryRepository,
            IRepository<Core.ConversionFactors, Guid> conversionFactorsRepository,
            IRepository<ProductEmissions, Guid> productEmissionsRepository, 
            IRepository<CustomerProduct, Guid> customerProductRepository, 
            IRepository<Core.EmissionsSummary, Guid> emissionsSummaryRepository) : base(activityDataRepository)

        {
            _activityDataRepository = activityDataRepository;
            _unitRepository = unitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _organizationRepository = organizationRepository;
            _activityTypeRepository = activityTypeRepository;
            _fuelTypeRepository = fuelTypeRepository;
            _purchasedEnergyDataRepository = purchasedEnergyDataRepository;
            _stationaryCombustionDataRepository = stationaryCombustionDataRepository;
            _mobileCombustionDataRepository = mobileCombustionDataRepository;
            _emissionsSourceRepository = emissionsSourceRepository;
            _emissionsRepository = emissionsRepository;
            _emissionGroupsRepository = emissionGroupsRepository;
            _clientFactory = clientFactory;
            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _notificationPublisher = notificationPublisher;
            _emissionSourceRepository = emissionSourceRepository;
            _purchasedProductDataRepository = purchasedProductDataRepository;
            _productRepository = productRepository;
            _businessTravelDataRepository = businessTravelDataRepository;
            _employeeCommuteDataRepository = employeeCommuteDataRepository;
            _transportAndDistributionDataRepository = transportAndDistributionDataRepository;
            _fugitiveEmissionsDataRepository = fugitiveEmissionsDataRepository;
            _greenhouseGasesRepository = greenhouseGasesRepository;
            _wasteGeneratedDataRepository = wasteGeneratedDataRepository;
            _endOfLifeTreatmentDataRepository = endOfLifeTreatmentDataRepository;
            _emissionFactorRepository = emissionFactorRepository;
            _emissionFactorsLibraryRepository = emissionFactorsLibraryRepository;
            _conversionFactorsRepository = conversionFactorsRepository;
            _productEmissionsRepository = productEmissionsRepository;
            _customerProductRepository = customerProductRepository;
            _emissionsSummaryRepository = emissionsSummaryRepository;
        }

        /// <summary>
        /// Method to upload activity data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ActivityData> AddActivityByEmissionSourceAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                var result = new ActivityData();
                var emissionSource = _emissionsSourceRepository.FirstOrDefault(x => x.Id == input.EmissionSourceId);
                //TODO: check if this is desired. If we have multiple Activity types that belong to the same Emissions Source, 
                // we should check for the specific activity type. We can have Stationary combustion based on purchased natural gas
                // or Stationary combustion based on fuel usage or even on amount spent. Doing FirstOrDefault() will return first element if we have multiple.
                var activitTypeId = _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId).Id;
                var activityId = Guid.NewGuid();

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                switch (emissionSource.Name)
                {
                    case "Mobile Combustion":
                        var activityModel = new MobileCombustionData
                        {
                            Id = activityId,
                            Name = input.Name,
                            Quantity = (float)input.Quantity,
                            UnitId = input.UnitId,
                            TransactionId = input.TransactionSource,
                            TransactionDate = ((DateTime)input.ConsumptionEnd),
                            ActivityTypeId = activitTypeId,
                            Description = input.Description,
                            IsActive = true,
                            DataQualityType = DataQualityType.Actual,
                            IndustrialProcessId = 1,
                            ConsumptionEnd = ((DateTime)input.ConsumptionEnd),
                            ConsumptionStart = ((DateTime)input.ConsumptionStart),
                            OrganizationUnitId = input.OrganizationUnitId,
                            isProcessed = false,
                            EmissionGroupId = input.EmissionGroupId,
                            EmissionFactorId = input.EmissionFactorId,
                            Status = input.ActivityDataStatus,

                        };
                        result = await _mobileCombustionDataRepository.InsertAsync(activityModel);
                        break;
                    case "Purchased Electricity":
                        var purchaseElectricityModel = new PurchasedEnergyData
                        {
                            Id = activityId,
                            Name = input.Name,
                            Quantity = (float)input.Quantity,
                            UnitId = input.UnitId,
                            TransactionId = input.TransactionSource,
                            TransactionDate = ((DateTime)input.ConsumptionEnd),
                            ActivityTypeId = activitTypeId,
                            Description = input.Description,
                            IsActive = true,
                            DataQualityType = DataQualityType.Actual,
                            IndustrialProcessId = 1,
                            ConsumptionEnd = ((DateTime)input.ConsumptionEnd),
                            ConsumptionStart = ((DateTime)input.ConsumptionStart),
                            OrganizationUnitId = input.OrganizationUnitId,
                            isProcessed = false,
                            EnergyType = EnergyType.Electricity,
                            EmissionGroupId = input.EmissionGroupId,
                            EmissionFactorId = input.EmissionFactorId,
                            Status = input.ActivityDataStatus,
                            EnergyMix = input.EnergyMix
                        };
                        result = await _purchasedEnergyDataRepository.InsertAsync(purchaseElectricityModel);
                        break;

                    case "Stationary Combustion":
                        //TODO: To check if this is desired. We should maybe check based on the Unit selected.
                        // The Unit can be Liters of.... In that case we should select appropiate Fuel type.
                        var fuelType = _fuelTypeRepository.GetAll().Where(x => x.Name == "Natural Gas").FirstOrDefault();
                        var stationaryCombustionModel = new StationaryCombustionData
                        {
                            Id = activityId,
                            Name = input.Name,
                            Quantity = (float)input.Quantity,
                            UnitId = input.UnitId,
                            TransactionId = input.TransactionSource,
                            TransactionDate = ((DateTime)input.ConsumptionEnd),
                            ActivityTypeId = activitTypeId,
                            Description = input.Description,
                            IsActive = true,
                            DataQualityType = DataQualityType.Actual,
                            IndustrialProcessId = 1,
                            ConsumptionEnd = ((DateTime)input.ConsumptionEnd),
                            ConsumptionStart = ((DateTime)input.ConsumptionStart),
                            OrganizationUnitId = input.OrganizationUnitId,
                            FuelTypeId = fuelType?.Id,
                            isProcessed = false,
                            EmissionGroupId = input.EmissionGroupId,
                            EmissionFactorId = input.EmissionFactorId,
                            Status = input.ActivityDataStatus

                        };
                        result = await _stationaryCombustionDataRepository.InsertAsync(stationaryCombustionModel);
                        break;
                }
                await CurrentUnitOfWork.SaveChangesAsync();

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
                    ActivityDataId = activityId,
                };

                await _emissionsRepository.InsertAsync(emissionsModel);

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                _logger.LogError($"Method: AddActivityByEmissionSourceAsync - Exception: {userEx.Message}");
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddActivityByEmissionSourceAsync - Exception: {ex}");
                return null;
            }

        }

        /// <summary>
        /// method to upload Update activity data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ActivityData> UpdateActivityByEmissionSourceAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var result = new ActivityData();
                var emissionSource = _emissionsSourceRepository.FirstOrDefault(x => x.Id == input.EmissionSourceId);
                //TODO: check if this is desired. If we have multiple Activity types that belong to the same Emissions Source, 
                // we should check for the specific activity type. We can have Stationary combustion based on purchased natural gas
                // or Stationary combustion based on fuel usage or even on amount spent. Doing FirstOrDefault() will return first element if we have multiple.
                var activitTypeId = _activityTypeRepository.FirstOrDefault(x => x.EmissionsSourceId == input.EmissionSourceId).Id;
                var emissionRow = _emissionsRepository.FirstOrDefault(x => x.ActivityDataId == input.Id);
                switch (emissionSource.Name)
                {
                    case "Mobile Combustion":

                        var mobileCombustionModel = _mobileCombustionDataRepository.FirstOrDefault(x => x.Id == input.Id);

                        if (mobileCombustionModel != null)
                        {
                            mobileCombustionModel.Name = input.Name;
                            mobileCombustionModel.Quantity = (float)input.Quantity;
                            mobileCombustionModel.UnitId = input.UnitId;
                            mobileCombustionModel.TransactionId = input.TransactionSource;
                            mobileCombustionModel.TransactionDate = (DateTime)input.ConsumptionEnd;
                            mobileCombustionModel.ActivityTypeId = activitTypeId;
                            mobileCombustionModel.Description = input.Description;
                            mobileCombustionModel.IsActive = true;
                            mobileCombustionModel.DataQualityType = DataQualityType.Actual;
                            mobileCombustionModel.IndustrialProcessId = 1;
                            mobileCombustionModel.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                            mobileCombustionModel.ConsumptionStart = (DateTime)input.ConsumptionStart;
                            mobileCombustionModel.OrganizationUnitId = input.OrganizationUnitId;
                            mobileCombustionModel.isProcessed = false;
                            mobileCombustionModel.EmissionGroupId = input.EmissionGroupId;
                            mobileCombustionModel.EmissionFactorId = input.EmissionFactorId;
                            mobileCombustionModel.EmissionFactorId = input.EmissionFactorId;
                            mobileCombustionModel.Status = input.ActivityDataStatus;
                            result = await _mobileCombustionDataRepository.UpdateAsync(mobileCombustionModel);
                        }
                        break;
                    case "Purchased Electricity":
                        var purchaseElectricityModel = _purchasedEnergyDataRepository.FirstOrDefault(x => x.Id == input.Id);
                        if (purchaseElectricityModel != null)
                        {
                            purchaseElectricityModel.Name = input.Name;
                            purchaseElectricityModel.Quantity = (float)input.Quantity;
                            purchaseElectricityModel.UnitId = input.UnitId;
                            purchaseElectricityModel.TransactionId = input.TransactionSource;
                            purchaseElectricityModel.TransactionDate = (DateTime)input.ConsumptionEnd;
                            purchaseElectricityModel.ActivityTypeId = activitTypeId;
                            purchaseElectricityModel.Description = input.Description;
                            purchaseElectricityModel.IsActive = true;
                            purchaseElectricityModel.DataQualityType = DataQualityType.Actual;
                            purchaseElectricityModel.IndustrialProcessId = 1;
                            purchaseElectricityModel.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                            purchaseElectricityModel.ConsumptionStart = (DateTime)input.ConsumptionStart;
                            purchaseElectricityModel.OrganizationUnitId = input.OrganizationUnitId;
                            purchaseElectricityModel.isProcessed = false;
                            purchaseElectricityModel.EnergyType = EnergyType.Electricity;
                            purchaseElectricityModel.EmissionGroupId = input.EmissionGroupId;
                            purchaseElectricityModel.EmissionFactorId = input.EmissionFactorId;
                            purchaseElectricityModel.Status = input.ActivityDataStatus;
                            purchaseElectricityModel.EnergyMix = input.EnergyMix;
                            result = await _purchasedEnergyDataRepository.UpdateAsync(purchaseElectricityModel);
                        }
                        break;

                    case "Stationary Combustion":
                        //TODO: To check if this is desired. We should maybe check based on the Unit selected.
                        // The Unit can be Liters of.... In that case we should select appropiate Fuel type.
                        var fuelType = _fuelTypeRepository.GetAll().Where(x => x.Name == "Natural Gas").FirstOrDefault();
                        var stationaryCombustionModel = _stationaryCombustionDataRepository.FirstOrDefault(x => x.Id == input.Id);
                        if (stationaryCombustionModel != null)
                        {
                            stationaryCombustionModel.Name = input.Name;
                            stationaryCombustionModel.Quantity = (float)input.Quantity;
                            stationaryCombustionModel.UnitId = input.UnitId;
                            stationaryCombustionModel.TransactionId = input.TransactionSource;
                            stationaryCombustionModel.TransactionDate = (DateTime)input.ConsumptionEnd;
                            stationaryCombustionModel.ActivityTypeId = activitTypeId;
                            stationaryCombustionModel.Description = input.Description;
                            stationaryCombustionModel.IsActive = true;
                            stationaryCombustionModel.DataQualityType = DataQualityType.Actual;
                            stationaryCombustionModel.IndustrialProcessId = 1;
                            stationaryCombustionModel.ConsumptionEnd = (DateTime)input.ConsumptionEnd;
                            stationaryCombustionModel.ConsumptionStart = (DateTime)input.ConsumptionStart;
                            stationaryCombustionModel.OrganizationUnitId = input.OrganizationUnitId;
                            stationaryCombustionModel.FuelTypeId = fuelType?.Id;
                            stationaryCombustionModel.isProcessed = false;
                            stationaryCombustionModel.EmissionGroupId = input.EmissionGroupId;
                            stationaryCombustionModel.EmissionFactorId = input.EmissionFactorId;
                            stationaryCombustionModel.Status = input.ActivityDataStatus;
                            result = await _stationaryCombustionDataRepository.UpdateAsync(stationaryCombustionModel);
                        }
                        break;

                }

                //TODO: Handle the case in which there is no emission entry linked with the activity data
                if (emissionRow != null)
                {

                    var emissionData = _emissionsRepository.Get(emissionRow.Id);
                    emissionData.CO2E = calculatedEmissions;
                    emissionData.CO2EUnitId = input.CO2eUnitId;

                    await _emissionsRepository.UpdateAsync(emissionData);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                _logger.LogError($"Method: UpdateActivityByEmissionSourceAsync - Exception: {userEx.Message}");
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdateActivityByEmissionSourceAsync - Exception: {ex}");
                return null;
            }

        }



        /// <summary>
        /// Gets activity data by organization id, and consumption start and end dates
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ActivityDataVM>> GetActivityDataByOrganizationId(ActivityDataFilterModel input)
        {
            List<ActivityData> activityList = new();
            List<ActivityDataVM> activities = new();
            try
            {
                if (input.OrganizationId != Guid.Empty)
                {
                    activityList = await _activityDataRepository.GetAll()
                        .Include(x => x.ActivityType)
                        .ThenInclude(x => x.EmissionsSource)
                        .Include(x => x.OrganizationUnit)
                        .ThenInclude(x => x.Organization)
                        .Where(x => x.OrganizationUnit.Organization.Id == input.OrganizationId
                        && x.ActivityType.EmissionsSource.Id == input.EmissionSourceId
                        && (input.ConsumptionStart.Date <= x.ConsumptionEnd.Date && input.ConsumptionEnd.Date >= x.ConsumptionStart.Date))
                        .ToListAsync();
                }
                else
                {
                    activityList = await _activityDataRepository.GetAll()
                        .Include(x => x.OrganizationUnit)
                        .Include(x => x.ActivityType)
                        .ThenInclude(x => x.EmissionsSource)
                        .Where(x => x.ActivityType.EmissionsSource.Id == input.EmissionSourceId
                        && (input.ConsumptionStart.Date <= x.ConsumptionEnd.Date && input.ConsumptionEnd.Date >= x.ConsumptionStart.Date))
                        .ToListAsync();
                }
                // use join to activity data with emission on activityId with select 

                foreach (var activity in activityList)
                {
                    // this is performance killer if we have so much activities need to remove emissions from this model
                    var emission = _emissionsRepository.GetAll().FirstOrDefault(x => x.ActivityDataId == activity.Id);
                    ActivityDataVM model = new ActivityDataVM
                    {
                        Id = activity.Id,
                        Name = activity.Name,
                        Emission = emission != null ? emission.CO2E : 0,
                        Quantity = activity.Quantity,
                        IsProcessed = activity.isProcessed,
                        // stopgap measure for ClimateCamp Admin (no organization unit)
                        OrganizationUnit = activity.OrganizationUnit?.Name ?? "",
                        Period = activity.ConsumptionStart.ToShortDateString() + "-" + activity.ConsumptionEnd.ToShortDateString(),
                        ConsumptionStart = activity.ConsumptionStart,
                        ConsumptionEnd = activity.ConsumptionEnd,
                        TransactionDate = activity.TransactionDate,
                        UnitId = (int)activity.UnitId,
                        OrganizationUnitId = activity.OrganizationUnit.Id,
                        CO2e = emission?.CO2E,
                        CO2eUnitId = emission?.CO2EUnitId
                    };
                    activities.Add(model);
                }
                var result = new PagedResultDto<ActivityDataVM>()
                {
                    Items = ObjectMapper.Map<List<ActivityDataVM>>(activities.Where(x => !x.IsDeleted)),
                    TotalCount = activityList.Count
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: GetActivityDataByOrganizationId - Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Upload distance activity data in csv and retrun in json
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<CSVUploadModel>> UploadDistanceTravelActivityDataCSV(IFormFile file)
        {
            var activityList = new List<CSVUploadModel>();
            try
            {
                if (file == null || file.Length <= 0)
                    throw new Exception("File is empty");

                if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("File extension is not supported");

                var sb = new StringBuilder();
                string line;
                var file1 = new StreamReader(file.OpenReadStream());
                while ((line = await file1.ReadLineAsync()) != null) sb.AppendLine(line);
                /* Read the files into a List of strings */
                var engine = new FileHelperEngine<CSVUploadModel>();
                CSVUploadModel[] records = engine.ReadString(sb.ToString());
                var activityData = records.AsEnumerable().Skip(1);
                return activityData.ToList();

            }

            catch (Exception ex)
            {
                _logger.LogInformation($"Method: UploadDistanceTravelActivityDataCSV - Exception: {ex}");
                return activityList;
            }



        }

        /// <summary>
        /// Upload stationary combustion data in csv and retrun in json
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<StationaryCombustionCSVUploadDto>> UploadStationaryCombustionActivityDataCSV(IFormFile file)
        {
            _logger.LogInformation($"Method: UploadStationaryCombustionActivityDataCSV - Executing!");
            var activityList = new List<StationaryCombustionCSVUploadDto>();
            try
            {
                if (file == null || file.Length <= 0)
                    throw new Exception("File is empty");
                if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("File extension is not supported");
                var sb = new StringBuilder();
                string line;
                var file1 = new StreamReader(file.OpenReadStream());
                while ((line = await file1.ReadLineAsync()) != null) sb.AppendLine(line);
                /* Read the files into a List of strings */
                var engine = new FileHelperEngine<StationaryCombustionCSVUploadDto>();
                StationaryCombustionCSVUploadDto[] records = engine.ReadString(sb.ToString());
                var activityData = records.AsEnumerable().Skip(1);
                return activityData.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UploadStationaryCombustionActivityDataCSV - Exception: {ex}");
                return activityList;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SaveDistanceActivityData(List<CSVUploadModel> activityData, Guid? organizationId, int emissionSourceId, string fileName)
        {
            try
            {

                var cultureInfo = new CultureInfo("nl-BE");
                foreach (var activityDataItem in activityData)
                {
                    await AddDistanceActivityData(activityDataItem);
                }
                CalculationFunctionRequestModel model = new CalculationFunctionRequestModel
                {
                    EmissionSourceId = emissionSourceId,
                    OrganizationId = organizationId.ToString()
                };
                _ = CallPostCalculationFunction(model, _config.GetValue<string>("App:Functions:DistanceActivityCalculationFunction"));

                var totalCount = activityData.Count;
                var emissionsSourceName = _emissionsSourceRepository.GetAll().Where(x => x.Id == emissionSourceId).FirstOrDefault().Name;

                await PublishFileUploaded(fileName, totalCount, emissionsSourceName);
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Method: SaveDistanceActivityData - Exception: {ex}");
                return false;
            }
        }
        /// <summary>
        /// Save Distance Activity Data
        /// </summary>
        /// <param name="activityDataItem"></param>
        /// <returns></returns>
        public async Task AddDistanceActivityData(CSVUploadModel activityDataItem)
        {
            DateTime transactionDate = getFormattedDate(activityDataItem.transactionDate);
            var unitId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.quantityUnit.ToLower().ToString()).FirstOrDefault();
            var activityTypeId = _activityTypeRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.activityDataType.ToLower()).FirstOrDefault();
            var organizationUnitId = _organizationUnitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.organizationUnit.ToLower()).FirstOrDefault();
            var activityModel = new ActivityData
            {
                Name = activityDataItem.activityDataType,
                Quantity = float.Parse(activityDataItem.quantity),
                UnitId = unitId != null ? unitId.Id : _unitRepository.GetAll().Where(x => x.Name.ToLower() == "km").FirstOrDefault()?.Id,
                TransactionId = activityDataItem.transactionSource,
                TransactionDate = transactionDate,
                ActivityTypeId = activityTypeId?.Id,
                Description = activityDataItem.activityDescription,
                IsActive = true,
                DataQualityType = DataQualityType.Actual,
                IndustrialProcessId = 1,
                ConsumptionStart = transactionDate,
                ConsumptionEnd = transactionDate,
                OrganizationUnitId = organizationUnitId?.Id,
                isProcessed = false
            };
            if (!checkIfTransactionAlreadyExist(activityModel.TransactionDate, activityModel.SourceTransactionId, activityModel.Quantity, activityModel.OrganizationUnitId.Value))
                await _activityDataRepository.InsertAsync(activityModel);
        }

        /// <summary>
        /// save SationaryCombustion Data in activityData table 
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SaveStationaryCombustionActivityData(List<StationaryCombustionCSVUploadDto> activityData, Guid? organizationId, int emissionSourceId, string fileName)
        {
            _logger.LogInformation($"Method: SaveStationaryCombustionActivityData Executing");
            try
            {
                //  var cultureInfo = new CultureInfo("nl-BE");
                foreach (var activityDataItem in activityData)
                {
                    await InsertStationaryCombustionData(activityDataItem);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                CalculationFunctionRequestModel model = new CalculationFunctionRequestModel
                {
                    EmissionSourceId = emissionSourceId,
                    OrganizationId = organizationId.ToString()
                };
                // await  callStationaryCombustionCalculationFunctionAsync(model);
                var totalCount = activityData.Count;
                var emissionsSourceName = _emissionsSourceRepository.GetAll().Where(x => x.Id == emissionSourceId).FirstOrDefault().Name;

                await PublishFileUploaded(fileName, totalCount, emissionsSourceName);
                _logger.LogInformation($"Method: SaveStationaryCombustionActivityData Executed!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: SaveStationaryCombustionActivityData - Exception: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Insert Stationary Combustion Data
        /// </summary>
        /// <param name="activityDataItem"></param>
        /// <returns></returns>
        public async Task InsertStationaryCombustionData(StationaryCombustionCSVUploadDto activityDataItem)
        {
            DateTime transactionDate = getFormattedDate(activityDataItem.TransactionDate);
            var unitId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.QuantityUnit.ToLower().ToString()).FirstOrDefault();
            var activityTypeId = _activityTypeRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.ActivityDataType.ToLower()).FirstOrDefault();
            var organizationUnitId = _organizationUnitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.OrganizationUnit.ToLower()).FirstOrDefault();
            var fuelType = _fuelTypeRepository.GetAll().Where(x => x.Name == "Natural Gas").FirstOrDefault();
            var now = DateTime.UtcNow;
            var activityModel = new StationaryCombustionData
            {
                Name = activityDataItem.ActivityDataType,
                Quantity = float.Parse(activityDataItem.Quantity),
                UnitId = unitId != null ? unitId.Id : _unitRepository.GetAll().Where(x => x.Name.ToLower() == "km").FirstOrDefault()?.Id,
                TransactionId = activityDataItem.TransactionSource,
                TransactionDate = transactionDate,
                ActivityTypeId = activityTypeId?.Id,
                Description = activityDataItem.ActivityDescription,
                IsActive = true,
                DataQualityType = DataQualityType.Actual,
                IndustrialProcessId = 1,
                ConsumptionStart = now,
                ConsumptionEnd = now,
                OrganizationUnitId = organizationUnitId?.Id,
                isProcessed = false,
                FuelTypeId = fuelType?.Id
            };
            if (!checkIfTransactionAlreadyExist(activityModel.TransactionDate, activityModel.TransactionId, activityModel.Quantity, activityModel.OrganizationUnitId.Value))
                await _stationaryCombustionDataRepository.InsertAsync(activityModel);
        }

        /// <summary>
        /// call stationary Combustion Calculation Azure function
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CallStationaryCombustionCalculationFunction(Guid? organizationId, int emissionSourceId)
        {
            _logger.LogInformation($"Method: SaveStationaryCombustionActivityData Executing");
            try
            {
                CalculationFunctionRequestModel model = new CalculationFunctionRequestModel
                {
                    EmissionSourceId = emissionSourceId,
                    OrganizationId = organizationId.ToString()
                };
                await CallPostCalculationFunction(model, _config.GetValue<string>("App:Functions:StationaryCombustionCalculationFunction"));
                _logger.LogInformation($"Method: CallStationaryCombustionCalculationFunction Executed!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: SaveStationaryCombustionActivityData - Exception: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Call Generic Emission Calculation Azure function
        /// </summary>
        /// <param name="emissionFactorId"></param>
        /// <param name="quantity"></param>
        /// <param name="unitId"></param>
        /// <param name="userConversionFactor"></param>
        /// <param name="productId"></param>
        /// <returns cref="GenericEmissionCalculationResponseModel"></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GenericEmissionCalculationResponseModel> CallGenericEmissionCalculationFunction(Guid emissionFactorId, float quantity, int unitId, float userConversionFactor, Guid productId = default(Guid))
        {
            _logger.LogInformation($"Method: CallGenericEmissionCalculationFunction Executing");
            try
            {
                GenericEmissionCalculationRequestModel model = new GenericEmissionCalculationRequestModel
                {
                    EmissionFactorId = emissionFactorId,
                    Quantity = quantity,
                    UnitId = unitId,
                    UserConversionFactor = userConversionFactor,
                    ProductId = productId
                };

                var result = await CallPostGenericEmissionCalculationFunction(model, _config.GetValue<string>("App:Functions:GenericEmissionCalculationFunction"));
                _logger.LogInformation($"Method: CallGenericEmissionCaculation Executed!");
                var jsonString = await result.Content.ReadAsStringAsync();
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericEmissionCalculationResponseModel>(jsonString);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: CallGenericEmissionCalculationFunction - Exception: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Returns Purchased Electricity activity data that was read from file. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<PurchasedElectricityCSVUploadModel>> UploadPurchasedElectricityActivityDataCSV(IFormFile file)
        {
            _logger.LogInformation($"Method: UploadPurchasedElectricityActivityDataCSV - Executing!");
            var activityList = new List<PurchasedElectricityCSVUploadModel>();
            try
            {
                if (file == null || file.Length <= 0)
                    throw new ArgumentException("File is empty");

                if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("File extension is not supported");

                var sb = new StringBuilder();
                string line;
                var file1 = new StreamReader(file.OpenReadStream());
                while ((line = await file1.ReadLineAsync()) != null) sb.AppendLine(line);

                /* Read the files into a List of strings */
                var engine = new FileHelperEngine<PurchasedElectricityCSVUploadModel>();
                PurchasedElectricityCSVUploadModel[] records = engine.ReadString(sb.ToString());
                return records.Skip(1).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UploadPurchasedElectricityActivityDataCSV - Exception: {ex}");
                return activityList;
            }
        }

        /// <summary>
        /// Saves Purchased Electricity activity data into the database.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="organizationId"></param>
        /// <param name="emissionSourceId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SavePurchasedElectricityActivityData(List<PurchasedElectricityCSVUploadModel> activityData, Guid? organizationId, int emissionSourceId, string fileName)
        {
            try
            {

                foreach (var activityDataItem in activityData)
                {
                    await InsertPurchaseElectrictyData(activityDataItem);

                }
                CalculationFunctionRequestModel model = new()
                {
                    EmissionSourceId = emissionSourceId,
                    OrganizationId = organizationId.ToString()
                };
                _ = CallPostCalculationFunction(model, _config.GetValue<string>("App:Functions:PurchasedElectricityCalculationFunction"));
                var totalCount = activityData.Count;
                var emissionsSourceName = _emissionsSourceRepository.GetAll().Where(x => x.Id == emissionSourceId).FirstOrDefault().Name;
                await PublishFileUploaded(fileName, totalCount, emissionsSourceName);
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Method: SavePurchasedElectricityActivityData - Exception: {ex}");
                return false;
            }
        }
        /// <summary>
        /// Insert purchase elctricity type data
        /// </summary>
        /// <param name="activityDataItem"></param>
        /// <returns></returns>
        public async Task InsertPurchaseElectrictyData(PurchasedElectricityCSVUploadModel activityDataItem)
        {
            var cultureInfo = new CultureInfo("nl-BE");
            DateTime transactionDate = getFormattedDate(activityDataItem.TransactionDate);
            var unitId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.QuantityUnit.ToLower().ToString()).FirstOrDefault();
            var activityTypeId = _activityTypeRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.ActivityDataType.ToLower()).FirstOrDefault();
            var organizationUnitId = _organizationUnitRepository.GetAll().Where(x => x.Name.ToLower() == activityDataItem.OrganizationUnit.ToLower()).FirstOrDefault();
            var billingPeriod = DateTime.Parse(activityDataItem.ConsumptionStart, cultureInfo).ToString("MMMM yyyy");
            string description;
            description = $"Purchased Electricity - {billingPeriod} - {activityDataItem.MeterNumber}";

            var activityModel = new PurchasedEnergyData
            {
                Name = activityDataItem.ActivityDataType,
                Quantity = float.Parse(activityDataItem.Quantity),
                UnitId = unitId != null ? unitId.Id : _unitRepository.GetAll().Where(x => x.Name.ToLower() == "kWh").FirstOrDefault()?.Id,
                TransactionDate = transactionDate,
                ActivityTypeId = activityTypeId?.Id,
                Description = description,
                IsActive = true,
                DataQualityType = DataQualityType.Actual,
                IndustrialProcessId = 1,
                ConsumptionStart = getFormattedDate(activityDataItem.ConsumptionStart),
                ConsumptionEnd = getFormattedDate(activityDataItem.ConsumptionEnd),
                OrganizationUnitId = organizationUnitId?.Id,
                isProcessed = false,
                EnergyType = EnergyType.Electricity,
                EnergyMix = activityDataItem.EnergyMix
            };
            if (!checkIfTransactionAlreadyExist(activityModel.TransactionDate, activityModel.SourceTransactionId, activityModel.Quantity, activityModel.OrganizationUnitId.Value))
                await _purchasedEnergyDataRepository.InsertAsync(activityModel);
        }

        /// <summary>
        /// Returns manually added Emissions data that was read from file. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<ManualEmissionsCSVUploadDto>> UploadManualEmissionsDataCSV(IFormFile file)
        {
            _logger.LogInformation($"Method: UploadManualEmissionsDataCSV - Executing!");
            var activityList = new List<ManualEmissionsCSVUploadDto>();
            try
            {
                if (file == null || file.Length <= 0)
                    throw new ArgumentException("File is empty");

                if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("File extension is not supported");

                var sb = new StringBuilder();
                string line;
                var file1 = new StreamReader(file.OpenReadStream());
                while ((line = await file1.ReadLineAsync()) != null) sb.AppendLine(line);

                /* Read the files into a List of strings */
                var engine = new FileHelperEngine<ManualEmissionsCSVUploadDto>();
                ManualEmissionsCSVUploadDto[] records = engine.ReadString(sb.ToString());
                return records.Skip(1).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UploadManualEmissionsDataCSV - Exception: {ex}");
                return activityList;
            }
        }

        /// <summary>
        /// Saves a manually added Emission and its respective Activity Data to the DB
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> SaveManualEmissionsData(List<ManualEmissionsCSVUploadDto> activityData, string fileName)
        {
            try
            {
                foreach (var activityDataItem in activityData)
                {
                    Guid activityDataId = Guid.NewGuid();
                    Guid organizationUnitId = new Guid(activityDataItem.OrganizationUnitId);
                    var activityName = _activityTypeRepository.GetAll().Where(x => x.Id == int.Parse(activityDataItem.ActivityTypeId)).FirstOrDefault()?.Name;
                    var creatorUserId = int.Parse(activityDataItem.CreatorUserId);
                    DateTime creationTime = DateTime.UtcNow;
                    var activityDataUnitId = _unitRepository.GetAll().Where(x => x.Name == (activityDataItem.ActivityDataUnit)).FirstOrDefault()?.Id;
                    var CO2eUnit = _unitRepository.GetAll().Where(x => x.Name == activityDataItem.CO2eUnit).FirstOrDefault();

                    var activityModel = new ActivityData
                    {
                        Id = activityDataId,
                        OrganizationUnitId = organizationUnitId,
                        Name = activityDataItem.Name,
                        Description = $"Manually added - {activityName}",
                        ActivityTypeId = int.Parse(activityDataItem.ActivityTypeId),
                        IsActive = true,
                        DataQualityType = (DataQualityType)int.Parse(activityDataItem.DataQualityType),
                        ConsumptionStart = getFormattedDate(activityDataItem.ConsumptionStart),
                        ConsumptionEnd = getFormattedDate(activityDataItem.ConsumptionEnd),
                        TransactionDate = getFormattedDate(activityDataItem.TransactionDate),
                        Quantity = float.Parse(activityDataItem.ActivityDataQuantity),
                        UnitId = activityDataUnitId,
                        IndustrialProcessId = 1,
                        CreationTime = creationTime,
                        CreatorUserId = creatorUserId,
                        IsDeleted = false,
                        isProcessed = true,
                    };

                    var emissionActivityModel = new Emission
                    {
                        OrganizationUnitId = organizationUnitId,
                        EmissionsDataQualityScore = (EmissionsDataQualityScore?)int.Parse(activityDataItem.EmissionsDataQualityScore),
                        EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                        ResponsibleEntityID = Guid.Empty,
                        ResponsibleEntityType = 0,
                        IsActive = false,
                        CO2E = float.Parse(activityDataItem.CO2e),
                        CO2EUnit = CO2eUnit,
                        CreationTime = creationTime,
                        CreatorUserId = creatorUserId,
                        ActivityDataId = activityDataId
                    };

                    if (!CheckIfManualEmissionAlreadyExists(activityModel.TransactionDate, (float)emissionActivityModel.CO2E, (int)activityModel.ActivityTypeId, activityModel.OrganizationUnitId.Value))
                    {
                        await _activityDataRepository.InsertAsync(activityModel);
                        await _emissionsRepository.InsertAsync(emissionActivityModel);
                    }
                }

                var totalCount = activityData.Count;
                var emissionsSourceName = totalCount > 1 ? "multiple categories entries for multiple" : "a single category entry for an";
                await PublishFileUploaded(fileName, totalCount, emissionsSourceName);
                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Method: SaveManualEmissionsData - Exception: {ex}");
                return false;
            }
        }


        private async Task CallPostCalculationFunction(CalculationFunctionRequestModel model, string functionUrl)
        {
            var request = new StringContent(JsonSerializer.Serialize(model));
            var client = _clientFactory.CreateClient();
            await client.PostAsync(functionUrl, request);
        }

        private async Task<HttpResponseMessage> CallPostGenericEmissionCalculationFunction(GenericEmissionCalculationRequestModel model, string functionUrl)
        {
            var request = new StringContent(JsonSerializer.Serialize(model));
            var client = _clientFactory.CreateClient();
            var result = await client.PostAsync(functionUrl, request);

            return  result;
        }

        private async Task<HttpResponseMessage> CallPostRollForwardActivityDataFunction(RollForwardFunctionRequestModel model, string functionUrl)
        {
            var request = new StringContent(JsonSerializer.Serialize(model));
            var client = _clientFactory.CreateClient();
            var result = await client.PostAsync(functionUrl, request);

            return result;
        }

        /// <summary>
        /// Send a general notification to a specific user
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="totalCount"></param>
        /// <param name="emissionsSourceName"></param>
        /// <returns></returns>
        public async Task PublishFileUploaded(string fileName, int totalCount, string emissionsSourceName)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            Logger.Warn("Fileupload publish notification.");
            await _notificationPublisher.PublishAsync(
                NotificationTypes.FileUploaded,
                new MessageNotificationData(fileName + " has been uploaded with " + emissionsSourceName + " activity data and " + totalCount + " entries."),
                userIds: new[] { userIdentifier }
                );
        }



        /// <summary>
        /// check if transaction record already exist against provided input params
        /// </summary>
        /// <param name="transactionDate"></param>
        /// <param name="transcactionId"></param>
        /// <param name="quantity"></param>
        /// <param name="organizationUnitId"></param>
        /// <returns></returns>
        private bool checkIfTransactionAlreadyExist(DateTime transactionDate, string transcactionId, float quantity, Guid organizationUnitId)
        {
            return _activityDataRepository.GetAll()
                .Where(x => x.TransactionDate == transactionDate
                && x.TransactionId == transcactionId
                && x.Quantity == quantity
                && x.OrganizationUnitId == organizationUnitId).Any();
        }

        /// <summary>
        /// Checks if a manually added emission already exists in the databse.
        /// </summary>
        /// <param name="transactionDate"></param>
        /// <param name="CO2e"></param>
        /// <param name="activityTypeId"></param>
        /// <param name="organizationUnitId"></param>
        /// <returns></returns>
        private bool CheckIfManualEmissionAlreadyExists(DateTime transactionDate, float CO2e, int activityTypeId, Guid organizationUnitId)
        {
            return _emissionsRepository.GetAll()
                .Include(x => x.ActivityData)
                .Where(x => x.CO2E == CO2e
                && x.ActivityData.TransactionDate == transactionDate
                && x.ActivityData.ActivityTypeId == activityTypeId
                && x.OrganizationUnitId == organizationUnitId).Any();
        }

        private DateTime getFormattedDate(string dateTime)
        {
            var cultureInfo = new CultureInfo("nl-BE");
            var dateFormats = new string[] {
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "dd.MM.yyyy",
                "yyyy/MM/ddd",
                "yyyy-MM-dd",
                "yyyy.MM.dd"};
            DateTime transactionDate = DateTime.ParseExact(dateTime.Split()[0], dateFormats, cultureInfo, DateTimeStyles.AdjustToUniversal);
            return transactionDate;
        }

        /// <summary>
        /// Get All Activity Data By Emission Group And Organization Id
        /// </summary>
        /// <param name="organizationId"></param>
        /// /// <param name="emissionGroupId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ActivityDataVM>> GetActivityDataByOrganizationAndEmissionGroup(Guid organizationId, Guid emissionGroupId, DateTime ConsumptionStart, DateTime ConsumptionEnd)
        {
            try
            {
                emissionGroupIdList = new List<Guid>();

                var units = await _unitRepository.GetAllListAsync();

                var emissionSources = await _emissionSourceRepository.GetAllListAsync();

                var emissionGroupTreeList = await _emissionGroupsRepository.GetAll()
                                            .Include(x => x.Children)
                                            .Where(x => x.OrganizationId == organizationId)
                                            .ToListAsync();

                emissionGroupTreeList = emissionGroupTreeList.Where(x => x.Id == emissionGroupId).ToList();

                foreach (var emissionGroup in emissionGroupTreeList)
                {
                    emissionGroupIdList.Add(emissionGroup.Id);
                    if (emissionGroup.Children != null && emissionGroup.Children.Any())
                        PopulateAllNestedChildEmissionGroupsIdList(emissionGroup.Children.ToList());
                }

                var activitiesData = await (from emissionGroups in _emissionGroupsRepository.GetAll().Include(x => x.Children)

                                            join activityData in _activityDataRepository.GetAll().Include(x => x.OrganizationUnit)
                                            on
                                            emissionGroups.Id equals activityData.EmissionGroupId

                                            join purchasedProductData in _purchasedProductDataRepository.GetAll()
                                            on
                                            activityData.Id equals purchasedProductData.Id into purchasedProductActData
                                            from purchasedProductData in purchasedProductActData.DefaultIfEmpty()

                                            join emissionFactor in _emissionFactorRepository.GetAll().Include(x => x.CO2EUnit).Include(x => x.Library)
                                            on activityData.EmissionFactorId equals emissionFactor.Id into emissionFactorActData
                                            from emissionFactor in emissionFactorActData.DefaultIfEmpty()

                                            join product in _productRepository.GetAll()
                                            on purchasedProductData.ProductId equals product.Id into purchasedProductPrdData
                                            from product in purchasedProductPrdData.DefaultIfEmpty()

                                            let productEmission = _productEmissionsRepository.GetAll()
                                            .Where(x => x.ProductId == product.Id &&
                                            (x.EmissionSourceType == (int)ProductEmissionTypeEnum.Benchmark ||
                                            x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product ||
                                            x.EmissionSourceType == (int)ProductEmissionTypeEnum.Organization)).FirstOrDefault()

                                            let maxEmission = _emissionsRepository.GetAll()
                                            .Where(emission => emission.ActivityDataId == activityData.Id)
                                            .Max(m => m.Version)

                                            join emission in _emissionsRepository.GetAll()
                                            on activityData.Id equals emission.ActivityDataId into actDataEmission
                                            from emission in actDataEmission.DefaultIfEmpty()

                                            join businessTravelData in _businessTravelDataRepository.GetAll()
                                            on
                                            activityData.Id equals businessTravelData.Id into businessTravelActData
                                            from businessTravelData in businessTravelActData.DefaultIfEmpty()

                                            join employeeCommuteData in _employeeCommuteDataRepository.GetAll()
                                            on
                                            activityData.Id equals employeeCommuteData.Id into employeeCommuteActData
                                            from employeeCommuteData in employeeCommuteActData.DefaultIfEmpty()

                                            join transportAndDistributionData in _transportAndDistributionDataRepository.GetAll()
                                            on
                                            activityData.Id equals transportAndDistributionData.Id into transportAndDistributionActData
                                            from transportAndDistributionData in transportAndDistributionActData.DefaultIfEmpty()

                                            join fugitiveEmissionsData in _fugitiveEmissionsDataRepository.GetAll()
                                            on
                                            activityData.Id equals fugitiveEmissionsData.Id into fugitiveEmissionsActData
                                            from fugitiveEmissionsData in fugitiveEmissionsActData.DefaultIfEmpty()

                                            join wasteGeneratedData in _wasteGeneratedDataRepository.GetAll()
                                            on
                                            activityData.Id equals wasteGeneratedData.Id into wasteGeneratedActData
                                            from wasteGeneratedData in wasteGeneratedActData.DefaultIfEmpty()

                                            join endOfLifeTreatmentData in _endOfLifeTreatmentDataRepository.GetAll()
                                            on
                                            activityData.Id equals endOfLifeTreatmentData.Id into endOfLifeTreatmentActData
                                            from endOfLifeTreatmentData in endOfLifeTreatmentActData.DefaultIfEmpty()

                                            join greenhouseGasData in _greenhouseGasesRepository.GetAll()
                                            on fugitiveEmissionsData.GreenhouseGasId equals greenhouseGasData.Id into fugitiveEmissionsGreenhouseGas
                                            from greenhouseGasData in fugitiveEmissionsGreenhouseGas.DefaultIfEmpty()

                                            join purchasedEnergyData in _purchasedEnergyDataRepository.GetAll()
                                            on
                                            activityData.Id equals purchasedEnergyData.Id into purchasedEnergyActData
                                            from purchasedEnergyData in purchasedEnergyActData.DefaultIfEmpty()

                                            where
                                            emissionGroupIdList.Contains(activityData.EmissionGroupId ?? Guid.Empty) && !activityData.IsDeleted &&
                                            activityData.ConsumptionStart.Date >= ConsumptionStart.Date && activityData.ConsumptionEnd.Date <= ConsumptionEnd.Date &&
                                            emission.Version == maxEmission
                                            select
                                            new
                                            ActivityDataVM
                                            {
                                                Id = activityData.Id,
                                                Name = activityData.Name,
                                                EmissionGroupId = emissionGroups.Id,
                                                EmissionSourceId = emissionGroups.EmissionSourceId ?? 0,
                                                GroupName = emissionGroups.Name,
                                                Emission = emission != null ? emission.CO2E : 0,
                                                Quantity = activityData.Quantity,
                                                IsProcessed = activityData.isProcessed,
                                                OrganizationUnit = activityData.OrganizationUnit != null ? activityData.OrganizationUnit.Name : string.Empty,
                                                Period = activityData.ConsumptionStart.ToShortDateString() + "-" + activityData.ConsumptionEnd.ToShortDateString(),
                                                ConsumptionStart = activityData.ConsumptionStart,
                                                ConsumptionEnd = activityData.ConsumptionEnd,
                                                TransactionDate = activityData.TransactionDate,
                                                UnitId = (int)activityData.UnitId,
                                                OrganizationUnitId = activityData.OrganizationUnit.Id,
                                                CO2e = emission != null ? emission.CO2E : 0,
                                                CO2eUnitId = emission != null ? emission.CO2EUnitId : 0,
                                                ProductId = purchasedProductData != null ? purchasedProductData.ProductId : Guid.Empty,
                                                PurchasedProductId = purchasedProductData != null ? purchasedProductData.Id : Guid.Empty, // will be same as activity data Id
                                                Status = product != null ? product.Status : 0,
                                                ProductCode = purchasedProductData != null ? purchasedProductData.ProductCode : null,
                                                ProductCO2eq = productEmission != null ? productEmission.CO2eq : null, //product != null ? product.CO2eq : null,
                                                ProductCO2eqUnitId = productEmission != null ? productEmission.CO2eqUnitId : null, //product != null ? product.CO2eqUnitId : null,
                                                ProductUnitId = product != null ? product.UnitId : null,
                                                VehicleTypeId = businessTravelData != null ? businessTravelData.VehicleTypeId : employeeCommuteData != null ? employeeCommuteData.VehicleTypeId : transportAndDistributionData != null ? transportAndDistributionData.VehicleTypeId : null,
                                                VehicleTypeName = businessTravelData != null ? businessTravelData.VehicleType.Name : employeeCommuteData != null ? employeeCommuteData.VehicleType.Name : transportAndDistributionData != null ? transportAndDistributionData.VehicleType.Name : null,
                                                GreenhouseGasId = fugitiveEmissionsData.GreenhouseGasId,
                                                GreenhouseGasCode = greenhouseGasData.Code,
                                                WasteTreatmentMethod = wasteGeneratedData != null ? wasteGeneratedData.WasteTreatmentMethod : endOfLifeTreatmentData != null ? endOfLifeTreatmentData.WasteTreatmentMethod : null,
                                                EmissionFactorId = emissionFactor != null ? emissionFactor.Id : null,
                                                EmissionFactorName = emissionFactor != null ? emissionFactor.Name : null,
                                                EmissionFactorCO2e = emissionFactor != null ? emissionFactor.CO2E : (activityData.Quantity > 0 ? emission.CO2E / activityData.Quantity : null),
                                                EmissionFactorCO2eUnitId = emissionFactor != null ? (emissionFactor.CO2EUnit != null ? emissionFactor.CO2EUnit.Id : null) : null,
                                                EmissionFactorLibraryName = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Name : null) : null,
                                                Year = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Year : null) : null,
                                                EnergyMix = purchasedEnergyData != null ? purchasedEnergyData.EnergyMix : null,
                                                SupplierOrganization = transportAndDistributionData != null ? transportAndDistributionData.SupplierOrganization : null,
                                                ActivityDataStatus = activityData.Status,
                                                Description = activityData.Description
                                            })
                                            .ToListAsync();

                activitiesData.ForEach(activityData =>
                {
                    activityData.EmissionFactorCO2eUnitName = activityData.EmissionFactorCO2eUnitId != null ? units.Single(x => x.Id == activityData.EmissionFactorCO2eUnitId).Name : null;
                    activityData.EmissionSourceName = activityData.EmissionSourceId > 0 ? emissionSources.Single(x => x.Id == activityData.EmissionSourceId).Name : null;
                });

                var result = new PagedResultDto<ActivityDataVM>()
                {
                    Items = ObjectMapper.Map<List<ActivityDataVM>>(activitiesData),
                    TotalCount = activitiesData.Count
                };
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetActivityDataByOrganizationAndEmissionGroup - Exception: {exception}");
                return null;
            }
        }

        /// <summary>
        /// Get Activity Data by organization Id and emissionGroup Id, grouped by emission group.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="emissionGroupId"></param>
        /// <param name="ConsumptionStart"></param>
        /// <param name="ConsumptionEnd"></param>
        /// <param name="year"></param>
        /// <param name="perUnitVolumeProduction"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GroupedActivityDataVM>> GetGroupedActivityData(Guid organizationId, Guid emissionGroupId, DateTime ConsumptionStart, DateTime ConsumptionEnd, int year, bool perUnitVolumeProduction)
        {
            try
            {
                emissionGroupIdList = new List<Guid>();

                var units = await _unitRepository.GetAllListAsync();

                var emissionSources = await _emissionSourceRepository.GetAllListAsync();

                var emissionGroupTreeList = await _emissionGroupsRepository.GetAll()
                                            .Include(x => x.Children)
                                            .Where(x => x.OrganizationId == organizationId)
                                            .ToListAsync();

                emissionGroupTreeList = emissionGroupTreeList.Where(x => x.Id == emissionGroupId).ToList();

                foreach (var emissionGroup in emissionGroupTreeList)
                {
                    emissionGroupIdList.Add(emissionGroup.Id);
                    if (emissionGroup.Children != null && emissionGroup.Children.Any())
                        PopulateAllNestedChildEmissionGroupsIdList(emissionGroup.Children.ToList());
                }

                float? sumProductionQuantity = 0;
                int productionQunatityUnit = 0, kgId = 0, tId = 0;
                string productionQuantityUnitName = string.Empty;

                if (perUnitVolumeProduction)
                {

                    //Get All Organization Units in heirarchical order for the organization
                    var organizationUnitsList = await _organizationUnitRepository.GetAll()
                                            .Where(x => x.OrganizationId == organizationId)
                                            .Select(x => new { x.Id, x.ParentOrganizationUnitId })
                                            .ToListAsync();

                    //Get the top level or parent organization unit
                    var parentOrganizationUnit = organizationUnitsList.First(x => x.ParentOrganizationUnitId == null);

                    //Check if top level organization unit data exists against selected period
                    var parentOrganizationUnitData = await _emissionsSummaryRepository.GetAll()
                                             .Where(x => x.OrganizationUnitId == parentOrganizationUnit.Id && x.Period == year)
                                             .ToListAsync();

                    if (parentOrganizationUnitData.Any())
                    {
                        sumProductionQuantity = parentOrganizationUnitData.Sum(x => x.ProductionQuantity);

                        productionQunatityUnit = parentOrganizationUnitData.First().ProductionMetricId;

                        productionQuantityUnitName = _unitRepository.GetAll().Where(x => x.Id == productionQunatityUnit).Single().Name;
                    }

                    //If production quantity does not exists for the top level organization unit against selected period
                    //then get the production quantity against all the child level organization units against the selected period
                    if (sumProductionQuantity == 0)
                    {

                        var childOrganizationUnits = organizationUnitsList
                                                    .Where(x => x.ParentOrganizationUnitId != null)
                                                    .Select(x => x.Id)
                                                    .ToList();

                        sumProductionQuantity = await _emissionsSummaryRepository.GetAll()
                                             .Where(x => childOrganizationUnits.Contains(x.OrganizationUnitId) && x.Period == year)
                                             .SumAsync(x => x.ProductionQuantity);

                    }

                }

                    kgId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "kg").Single().Id;
                    tId = _unitRepository.GetAll().Where(x => x.Name.ToLower() == "t").Single().Id;

                var activitiesData = await (from emissionGroups in _emissionGroupsRepository.GetAll().Include(x => x.Children)

                                            join activityData in _activityDataRepository.GetAll().Include(x => x.OrganizationUnit)
                                            on
                                            emissionGroups.Id equals activityData.EmissionGroupId

                                            join purchasedProductData in _purchasedProductDataRepository.GetAll()
                                            on
                                            activityData.Id equals purchasedProductData.Id into purchasedProductActData
                                            from purchasedProductData in purchasedProductActData.DefaultIfEmpty()

                                            join emissionFactor in _emissionFactorRepository.GetAll().Include(x => x.CO2EUnit).Include(x => x.Library)
                                            on activityData.EmissionFactorId equals emissionFactor.Id into emissionFactorActData
                                            from emissionFactor in emissionFactorActData.DefaultIfEmpty()

                                            join product in _productRepository.GetAll()
                                            .Include(x => x.Organization)
                                            .Include(x => x.CustomerProducts.Where(custProd => custProd.OrganizationId == organizationId))
                                            on purchasedProductData.ProductId equals product.Id into purchasedProductPrdData
                                            from product in purchasedProductPrdData.DefaultIfEmpty()

                                            //only getting product and organization product emissions. Benchmark emissions are added from the emission linked to the purchased product
                                            let productEmission = _productEmissionsRepository.GetAll()
                                            .Where(x => x.ProductId == product.Id &&
                                            (x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product ||
                                            x.EmissionSourceType == (int)ProductEmissionTypeEnum.Organization)).FirstOrDefault()

                                            let maxEmission = _emissionsRepository.GetAll()
                                            .Where(emission => emission.ActivityDataId == activityData.Id)
                                            .Max(m => m.Version)

                                            join emission in _emissionsRepository.GetAll()
                                            on activityData.Id equals emission.ActivityDataId into actDataEmission
                                            from emission in actDataEmission.DefaultIfEmpty()

                                            join businessTravelData in _businessTravelDataRepository.GetAll()
                                            on
                                            activityData.Id equals businessTravelData.Id into businessTravelActData
                                            from businessTravelData in businessTravelActData.DefaultIfEmpty()

                                            join employeeCommuteData in _employeeCommuteDataRepository.GetAll()
                                            on
                                            activityData.Id equals employeeCommuteData.Id into employeeCommuteActData
                                            from employeeCommuteData in employeeCommuteActData.DefaultIfEmpty()

                                            join transportAndDistributionData in _transportAndDistributionDataRepository.GetAll()
                                            on
                                            activityData.Id equals transportAndDistributionData.Id into transportAndDistributionActData
                                            from transportAndDistributionData in transportAndDistributionActData.DefaultIfEmpty()

                                            join fugitiveEmissionsData in _fugitiveEmissionsDataRepository.GetAll()
                                            on
                                            activityData.Id equals fugitiveEmissionsData.Id into fugitiveEmissionsActData
                                            from fugitiveEmissionsData in fugitiveEmissionsActData.DefaultIfEmpty()

                                            join wasteGeneratedData in _wasteGeneratedDataRepository.GetAll()
                                            on
                                            activityData.Id equals wasteGeneratedData.Id into wasteGeneratedActData
                                            from wasteGeneratedData in wasteGeneratedActData.DefaultIfEmpty()

                                            join endOfLifeTreatmentData in _endOfLifeTreatmentDataRepository.GetAll()
                                            on
                                            activityData.Id equals endOfLifeTreatmentData.Id into endOfLifeTreatmentActData
                                            from endOfLifeTreatmentData in endOfLifeTreatmentActData.DefaultIfEmpty()

                                            join greenhouseGasData in _greenhouseGasesRepository.GetAll()
                                            on fugitiveEmissionsData.GreenhouseGasId equals greenhouseGasData.Id into fugitiveEmissionsGreenhouseGas
                                            from greenhouseGasData in fugitiveEmissionsGreenhouseGas.DefaultIfEmpty()

                                            join purchasedEnergyData in _purchasedEnergyDataRepository.GetAll()
                                            on
                                            activityData.Id equals purchasedEnergyData.Id into purchasedEnergyActData
                                            from purchasedEnergyData in purchasedEnergyActData.DefaultIfEmpty()

                                            join productConversionFactor in _conversionFactorsRepository.GetAll()
                                            on 
                                            product.Id equals productConversionFactor.ProductId into productConversionFactorData
                                            from productConversionFactor in productConversionFactorData.Where(x => x.IsActive == true).DefaultIfEmpty()

                                            where
                                            emissionGroupIdList.Contains(activityData.EmissionGroupId ?? Guid.Empty) && !activityData.IsDeleted &&
                                            activityData.ConsumptionStart.Date >= ConsumptionStart.Date && activityData.ConsumptionEnd.Date <= ConsumptionEnd.Date &&
                                             emission.Version == maxEmission
                                            select
                                            new
                                            ActivityDataVM
                                            {
                                                Id = activityData.Id,
                                                Name = activityData.Name,
                                                EmissionGroupId = emissionGroups.Id,
                                                EmissionSourceId = emissionGroups.EmissionSourceId ?? 0,
                                                GroupName = emissionGroups.Name,
                                                Emission = emission != null ? emission.CO2E : 0,
                                                Quantity = activityData.Quantity,
                                                IsProcessed = activityData.isProcessed,
                                                OrganizationUnit = activityData.OrganizationUnit != null ? activityData.OrganizationUnit.Name : string.Empty,
                                                Period = activityData.ConsumptionStart.ToShortDateString() + "-" + activityData.ConsumptionEnd.ToShortDateString(),
                                                ConsumptionStart = activityData.ConsumptionStart,
                                                ConsumptionEnd = activityData.ConsumptionEnd,
                                                TransactionDate = activityData.TransactionDate,
                                                UnitId = (int)activityData.UnitId,
                                                OrganizationUnitId = activityData.OrganizationUnit.Id,
                                                CO2e = emission != null ? emission.CO2E : 0,
                                                CO2eUnitId = emission != null ? emission.CO2EUnitId : 0,
                                                ProductId = purchasedProductData != null ? purchasedProductData.ProductId : Guid.Empty,
                                                PurchasedProductId = purchasedProductData != null ? purchasedProductData.Id : Guid.Empty, // will be same as activity data Id
                                                Status = product != null ? product.Status : 0,
                                                ProductCode = GetProductCode(purchasedProductData,product.CustomerProducts),
                                                ProductCO2eq = productEmission != null ? productEmission.CO2eq : null, 
                                                ProductCO2eqUnitId = productEmission != null ? productEmission.CO2eqUnitId : null,
                                                ProductUnitId = product != null ? product.UnitId : null,
                                                VehicleTypeId = businessTravelData != null ? businessTravelData.VehicleTypeId : employeeCommuteData != null ? employeeCommuteData.VehicleTypeId : transportAndDistributionData != null ? transportAndDistributionData.VehicleTypeId : null,
                                                VehicleTypeName = businessTravelData != null ? businessTravelData.VehicleType.Name : employeeCommuteData != null ? employeeCommuteData.VehicleType.Name : transportAndDistributionData != null ? transportAndDistributionData.VehicleType.Name : null,
                                                GreenhouseGasId = fugitiveEmissionsData.GreenhouseGasId,
                                                GreenhouseGasCode = greenhouseGasData.Code,
                                                WasteTreatmentMethod = wasteGeneratedData != null ? wasteGeneratedData.WasteTreatmentMethod : endOfLifeTreatmentData != null ? endOfLifeTreatmentData.WasteTreatmentMethod : null,
                                                EmissionFactorId = emissionFactor != null ? emissionFactor.Id : null,
                                                EmissionFactorName = emissionFactor != null ? emissionFactor.Name : null,
                                                EmissionFactorCO2e = emissionFactor != null ? emissionFactor.CO2E :
                                                                     (activityData.Quantity > 0 ?
                                                                     ( productConversionFactor.ConversionUnit == null ?
                                                                     emission.CO2E / activityData.Quantity :
                                                                     emission.CO2E / (activityData.Quantity * productConversionFactor.ConversionFactor)) :
                                                                     null),
                                                EmissionFactorCO2eUnitId = emissionFactor != null ? (emissionFactor.CO2EUnit != null ? emissionFactor.CO2EUnit.Id : null) : null,
                                                EmissionFactorLibraryName = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Name : null) : null,
                                                Year = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Year : null) : null,
                                                EnergyMix = purchasedEnergyData != null ? purchasedEnergyData.EnergyMix : null,
                                                SupplierOrganization = transportAndDistributionData != null ? transportAndDistributionData.SupplierOrganization : null,
                                                ActivityDataStatus = activityData.Status,
                                                Description = activityData.Description,
                                                SupplierName =  product != null ? GetProductSupplierName(product) : null,
                                                EmissionType = productEmission != null ? productEmission.EmissionSourceType : null,
                                            })
                        .ToListAsync();
              
                activitiesData.ForEach(activityData =>
                {
                    activityData.EmissionFactorCO2eUnitName = activityData.EmissionFactorCO2eUnitId != null ? units.Single(x => x.Id == activityData.EmissionFactorCO2eUnitId).Name : null;
                    activityData.EmissionSourceName = activityData.EmissionSourceId > 0 ? emissionSources.Single(x => x.Id == activityData.EmissionSourceId).Name : null;
                    activityData.PerUnitCO2e = perUnitVolumeProduction ? GetPerUnitCO2e(activityData, sumProductionQuantity, productionQunatityUnit, kgId, tId) : 0;
                    activityData.PerUnitCO2eUnit = perUnitVolumeProduction ? (activityData.CO2eUnitId == kgId ? "kg" : "t") : string.Empty;
                    activityData.ProductionQuantityUnit = productionQuantityUnitName;

                });


                List<GroupedActivityDataVM> groupedActivityDataList = GroupActivitData(emissionGroupTreeList, activitiesData);

                var result = new PagedResultDto<GroupedActivityDataVM>()
                {
                    Items = groupedActivityDataList,
                    TotalCount = activitiesData.Count
                };
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetActivityDataByOrganizationAndEmissionGroup - Exception: {exception}");
                return null;
            }
        }

        private static float? GetPerUnitCO2e(ActivityDataVM activityData, float? sumProductionQuantity, int productionQuantityUnit, int kgId, int tId)
        {
            if (activityData.CO2e == 0 || activityData.CO2e == null)
            {
                return 0;
            }

            if (sumProductionQuantity == 0)
            {
                return activityData.CO2e;
            }

            double activityDataDays = Math.Round((activityData.ConsumptionEnd - activityData.ConsumptionStart).Value.TotalDays / 365, 5);

            activityDataDays = activityDataDays == 0 ? 1 : activityDataDays;

            float? convertedProductionQuantity = 0;

            if(activityData.CO2eUnitId == productionQuantityUnit)
            {
                convertedProductionQuantity = sumProductionQuantity;
            }
            else
            {
                if(activityData.CO2eUnitId == kgId && productionQuantityUnit == tId)
                {
                    convertedProductionQuantity = sumProductionQuantity * 1000;
                }
                else if(activityData.CO2eUnitId == tId && productionQuantityUnit == kgId)
                {
                    convertedProductionQuantity = sumProductionQuantity / 1000;
                }
                else
                {
                    convertedProductionQuantity = sumProductionQuantity;
                }
            }

            return (float?)(activityData.CO2e * activityDataDays * (1 / convertedProductionQuantity));

        }

        private static string GetProductSupplierName(Product product)
        {
            if(product.Organization != null)
            {
                return product.Organization.Name;
            }
            else
            {
                return null;
            }
        }

        private static string GetProductCode(PurchasedProductsData purchasedProduct, ICollection<CustomerProduct> customerProducts)
        {
            if (!customerProducts.Any())
            {
                if(purchasedProduct != null)
                {
                    return purchasedProduct.ProductCode;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return customerProducts.First().ProductCode;
            }
        }

        //group activity data based on emission group
        private List<GroupedActivityDataVM> GroupActivitData(List<EmissionGroups> emissionGroups, List<ActivityDataVM> activities)
        {
            try
            {
                Dictionary<Guid, List<ActivityDataVM>> filteredActivities = new Dictionary<Guid, List<ActivityDataVM>>();
                foreach (var emissionGroup in emissionGroups)
                {
                    filteredActivities[emissionGroup.Id] = activities.Where(x => x.EmissionGroupId == emissionGroup.Id).ToList();
                }

                List<GroupedActivityDataVM> groupedActivityDataVMs = new List<GroupedActivityDataVM>();

                foreach (var emissionGroup in emissionGroups)
                {
                    var groupedActivityDataVM = new GroupedActivityDataVM
                    {
                        EmissionGroupId = emissionGroup.Id,
                        EmissionGroupName = emissionGroup.Name,
                        ParentEmissionGroupId = emissionGroup.ParentEmissionGroupId,
                        Activities = filteredActivities[emissionGroup.Id]
                    };
                    groupedActivityDataVM.Children = GroupActivitData(emissionGroup.Children.ToList(), activities).ToList();
                    groupedActivityDataVMs.Add(groupedActivityDataVM);
                }

                return (List<GroupedActivityDataVM>)groupedActivityDataVMs;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GroupActivitData - Exception: {exception}");
                return null;
            }
        }


        public async Task<List<EmissionFactorVM>> GetAllEmissionFactors(Guid organizationId)
        {
            try
            {
                var currentOrganizationEmissionFactorLibraryId = default(Guid?);

                if (organizationId != Guid.Empty)
                    currentOrganizationEmissionFactorLibraryId = _organizationRepository.Get(organizationId).EmissionsFactorsLibraryId;

                var orphanEmissionFactorLibraryIdList = await (from
                                                               emissionFactorLibrary in _emissionFactorsLibraryRepository.GetAll()
                                                               where !_organizationRepository.GetAll().Any(x => x.EmissionsFactorsLibraryId == emissionFactorLibrary.Id)
                                                               select emissionFactorLibrary.Id
                                                               ).ToListAsync();

                var emissionFactorsResultSet = await (from emissionFactor in _emissionFactorRepository
                                                      .GetAll()
                                                      .Where(x => (currentOrganizationEmissionFactorLibraryId != Guid.Empty && currentOrganizationEmissionFactorLibraryId != null) ?
                                                                  (x.Library.Id == currentOrganizationEmissionFactorLibraryId || orphanEmissionFactorLibraryIdList.Contains(x.Library.Id)) :
                                                                  orphanEmissionFactorLibraryIdList.Contains(x.Library.Id))
                                                      .Include(x => x.EmissionsSource)
                                                      .Include(x => x.Library)
                                                      .Include(x => x.Unit)
                                                      select
                                                      new EmissionFactorVM
                                                      {
                                                          Id = emissionFactor.Id,
                                                          EmissionSourceId = emissionFactor.EmissionsSource != null ? emissionFactor.EmissionsSource.Id : null,
                                                          Name = (!string.IsNullOrEmpty(emissionFactor.Name) && emissionFactor.Library != null && !string.IsNullOrEmpty(emissionFactor.Library.Name)) ? string.Join(" - ", emissionFactor.Name, emissionFactor.Library.Name) :
                                                                  string.IsNullOrEmpty(emissionFactor.Name) ? ((emissionFactor.Library != null && !string.IsNullOrEmpty(emissionFactor.Library.Name)) ? emissionFactor.Library.Name : string.Empty) : emissionFactor.Name,
                                                          Description = !string.IsNullOrEmpty(emissionFactor.Description) ? emissionFactor.Description : null,
                                                          CO2e = emissionFactor.CO2E,
                                                          CO2eUnitId = emissionFactor.CO2EUnit.Id,
                                                          CO2 = emissionFactor.CO2,
                                                          CO2UnitId = emissionFactor.CO2Unit.Id,
                                                          UnitId = emissionFactor.Unit.Id,
                                                          EmissionFactorLibrary = emissionFactor.Library != null ? new EmissionFactorLibrary
                                                          {
                                                              Id = emissionFactor.Library.Id,
                                                              Name = emissionFactor.Library.Name,
                                                              Year = emissionFactor.Library.Year
                                                          } : new EmissionFactorLibrary(),
                                                          Unit = emissionFactor.Unit,
                                                      }).ToListAsync();

                emissionFactorsResultSet.ForEach(emissionFactor =>
                {
                    emissionFactor.Name = emissionFactor.Name.Length > 35 ? emissionFactor.Name.Substring(0, 34) + "..." : emissionFactor.Name;
                });

                return emissionFactorsResultSet;

            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetAllEmissionFactors - Exception: {exception}");
                return null;
            }
        }

        public async Task<List<EmissionFactorVM>> GetAllEmissionFactorsByProductUnit(Guid productId, int productUnitGroup)
        {
            try
            {
                bool applyUnitFilter = false;
                var productUnitId = 0;

                var productConversionFactor = await _conversionFactorsRepository.GetAll().Where(x => x.ProductId == productId).FirstOrDefaultAsync();

                if (productConversionFactor != null)
                {
                    var conversionFactorUnitGroup = _unitRepository.Get(productConversionFactor.ConversionUnit).Group;

                    if (productUnitGroup == conversionFactorUnitGroup)
                    {
                        applyUnitFilter = true;
                        productUnitId = _productRepository.Get(productId).UnitId ?? 0;
                    }
                }

                var emissionFactorsResultSet = await (from emissionFactor in _emissionFactorRepository
                                                      .GetAll()
                                                      .Where(x => (applyUnitFilter ? x.Unit.Id == productUnitId : true) && x.EmissionsSource.Id == 10)
                                                      .Include(x => x.EmissionsSource)
                                                      .Include(x => x.Library)
                                                      .Include(x => x.Unit)
                                                      select
                                                      new EmissionFactorVM
                                                      {
                                                          Id = emissionFactor.Id,
                                                          EmissionSourceId = emissionFactor.EmissionsSource != null ? emissionFactor.EmissionsSource.Id : null,
                                                          Name = (!string.IsNullOrEmpty(emissionFactor.Name) && emissionFactor.Library != null && !string.IsNullOrEmpty(emissionFactor.Library.Name)) ? string.Join(" - ", emissionFactor.Name, emissionFactor.Library.Name) :
                                                                  string.IsNullOrEmpty(emissionFactor.Name) ? ((emissionFactor.Library != null && !string.IsNullOrEmpty(emissionFactor.Library.Name)) ? emissionFactor.Library.Name : string.Empty) : emissionFactor.Name,
                                                          Description = !string.IsNullOrEmpty(emissionFactor.Description) ? emissionFactor.Description : null,
                                                          CO2e = emissionFactor.CO2E,
                                                          CO2eUnitId = emissionFactor.CO2EUnit.Id,
                                                          CO2 = emissionFactor.CO2,
                                                          CO2UnitId = emissionFactor.CO2Unit.Id,
                                                          UnitId = emissionFactor.Unit.Id,
                                                          EmissionFactorLibrary = emissionFactor.Library != null ? new EmissionFactorLibrary
                                                          {
                                                              Id = emissionFactor.Library.Id,
                                                              Name = emissionFactor.Library.Name
                                                          } : new EmissionFactorLibrary(),
                                                          Unit = emissionFactor.Unit,
                                                      }).ToListAsync();

                emissionFactorsResultSet.ForEach(emissionFactor =>
                {
                    emissionFactor.Name = emissionFactor.Name.Length > 35 ? emissionFactor.Name.Substring(0, 34) + "..." : emissionFactor.Name;
                });

                return emissionFactorsResultSet;

            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: GetAllEmissionFactorsByProductUnit - Exception: {exception}");
                return null;
            }
        }

        private List<Guid> PopulateAllNestedChildEmissionGroupsIdList(List<EmissionGroups> emissionGroups)
        {
            emissionGroups.ForEach(emissionGroup =>
            {
                emissionGroupIdList.Add(emissionGroup.Id);

                if (emissionGroup.Children != null && emissionGroup.Children.Any())
                    PopulateAllNestedChildEmissionGroupsIdList(emissionGroup.Children.ToList());
            });

            return emissionGroupIdList;
        }

        /// <summary>
        /// Method used by the Roll Forward functionality. This calls the Azure function which will roll forward activity data.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="consumptionStart"></param>
        /// <param name="consumptionEnd"></param>
        /// <param name="targetPeriodStart"></param>
        /// <param name="targetPeriodEnd"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<bool> CallRollForwardActivityDataFunction(Guid organizationId, DateTime consumptionStart, DateTime consumptionEnd, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                if (await HasNoActivityData(organizationId, targetPeriodStart, targetPeriodEnd))
                {
                    _logger.LogInformation($"Method: CallRollForwardActivityDataFunction Executing");
                    RollForwardFunctionRequestModel model = new()
                    {
                        OrganizationId = organizationId,
                        ConsumptionStart = consumptionStart,
                        ConsumptionEnd = consumptionEnd,
                        TargetPeriodStart = targetPeriodStart,
                        TargetPeriodEnd = targetPeriodEnd,
                    };

                    var result = await CallPostRollForwardActivityDataFunction(model, _config.GetValue<string>("App:Functions:RollForwardActivityData"));
                    _logger.LogInformation($"Method: CallRollForwardActivityDataFunction Executed!");
                    var jsonString = await result.Content.ReadAsStringAsync();
                    var response = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(jsonString);
                    return response;
                }
                else
                {
                    throw new UserFriendlyException(
                        "The target period already has activity data.",
                        "In order to prevent duplicate data, the Roll Forward method cannot continue.");
                }
            }
            catch (UserFriendlyException userException)
            {
                throw new UserFriendlyException(userException.Message, userException.Details);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: CallRollForwardActivityDataFunction - Exception: {ex}", ex);
                return false;
            }
        }

        /// <summary>
        /// Checks if an Organization has existing, not deleted activity data for a specific date range. <br/>
        /// Used for Roll Forward functionality in order to prevent duplicate data being added.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="targetPeriodStart"></param>
        /// <param name="targetPeriodEnd"></param>
        /// <returns></returns>
        public async Task<bool> HasNoActivityData(Guid organizationId, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var hasActivityData = await _activityDataRepository.GetAll()
                    .Include(x => x.OrganizationUnit)
                    .AnyAsync(x => x.OrganizationUnit.OrganizationId == organizationId
                        && x.ConsumptionStart >= targetPeriodStart
                        && x.ConsumptionEnd <= targetPeriodEnd
                        && x.IsDeleted == false);
                return !hasActivityData;
            }
            catch (Exception exception)
            {
                _logger.LogError("Method: HasNoActivityData - Exception: {exception}", exception);
                return false;
            }
        }

        /// <summary>
        /// Takes an array with all the years and checks if each year has activity data or not.
        /// </summary>
        /// <param name="years"></param>
        /// <param name="organizationId"></param>
        /// <returns>
        /// A key/value list like "2022: true"
        /// </returns>
        public async Task<Dictionary<int, bool>> HasActivityDataYearlyPeriodAsync(List<int> years, Guid organizationId)
        {
            try
            {
            var activityData = await _activityDataRepository.GetAll()
                .Include(x => x.OrganizationUnit)
                .Where(x => x.OrganizationUnit.OrganizationId == organizationId
                    && (years.Contains(x.ConsumptionStart.Year)
                    || years.Contains(x.ConsumptionEnd.Year))).ToListAsync();
            var yearlyActivityData = years
                .ToDictionary(y => y, y => activityData.Any(x => x.ConsumptionStart.Year == y || x.ConsumptionEnd.Year == y));

            return yearlyActivityData;
            }
            catch (Exception exception)
            {
                _logger.LogError("Method: HasActivityDataYearlyPeriodAsync - Exception: {exception}", exception);
                return null;
            }
        }



        #region QUARTERLY PERIOD ACTIVITY DATA DROPDOWN
        public class YearData
        {
            public int Year { get; set; }
            public IEnumerable<int> Quarters { get; set; }
        }

        public class YearQuarterDataWithActivity : YearData
        {
            public IEnumerable<QuarterData> QuartersData { get; set; }
        }

        public class QuarterData
        {
            public int Quarter { get; set; }
            public bool HasActivityData { get; set; }
        }

        public List<YearQuarterDataWithActivity> HasActivityDataQuarterlyPeriod(List<YearData> data, Guid organizationId)
        {
            var result = new List<YearQuarterDataWithActivity>();

            // Create a list of start and end dates for each quarter
            var periods = new List<Tuple<DateTime, DateTime>>();
            foreach (var item in data)
            {
                var year = item.Year;
                var quarters = item.Quarters;


                foreach (var quarter in quarters)
                {
                    DateTime startDate = DateTime.MinValue, endDate = DateTime.MinValue;

                    switch (quarter)
                    {
                        case 1:
                            startDate = new DateTime(year, 1, 1);
                            endDate = new DateTime(year, 3, 31);
                            break;
                        case 2:
                            startDate = new DateTime(year, 4, 1);
                            endDate = new DateTime(year, 6, 30);
                            break;
                        case 3:
                            startDate = new DateTime(year, 7, 1);
                            endDate = new DateTime(year, 9, 30);
                            break;
                        case 4:
                            startDate = new DateTime(year, 10, 1);
                            endDate = new DateTime(year, 12, 31);
                            break;
                    }
                    periods.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
                }
            }


            var activityData = _activityDataRepository.GetAll()
                .Include(x => x.OrganizationUnit)
                .Select(x => new {x.ConsumptionStart, x.ConsumptionEnd, x.OrganizationUnit})
                .AsEnumerable()
                .Where(x => periods.Any(y => x.ConsumptionStart >= y.Item1 && x.ConsumptionEnd <= y.Item2) && x.OrganizationUnit.OrganizationId == organizationId).ToList();
            // Group the activity data by year and quarter
            var groupedData = activityData
                .Select(x => new { Year = x.ConsumptionStart.Year, Quarter = (x.ConsumptionStart.Month - 1) / 3 + 1 })
                .GroupBy(x => new { x.Year, x.Quarter });

            // Create the result list
            foreach (var item in data)
            {
                var year = item.Year;
                var quarters = item.Quarters;

                var quartersData = new List<QuarterData>();
                foreach (var quarter in quarters)
                {
                    var hasData = groupedData.AsEnumerable().Where(x => x.Key.Year == year && x.Key.Quarter == quarter).Select(x => x.Count()).FirstOrDefault() > 0;
                    quartersData.Add(new QuarterData()
                    {
                        Quarter = quarter,
                        HasActivityData = hasData
                    });
                }

                result.Add(new YearQuarterDataWithActivity()
                {
                    Year = year,
                    Quarters = item.Quarters,
                    QuartersData = quartersData
                });
            }

            return result;
        }
        #endregion

    }
}
