using Abp.Application.Services.Dto;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.Helpers;
using ClimateCamp.GHG.Calculations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;

namespace Mobile.Combustion.Calculation.DataService
{
    public class ActivityDataService : IActivityDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;

        private List<Guid> emissionGroupIdList;

        public ActivityDataService(ILoggerFactory loggerFactory,
            CommonDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger =   loggerFactory.CreateLogger<ActivityDataService>();
        }
        public async Task<ActivityData> SaveActivityData(ActivityData activity)
        {
            var isExist = _dbContext.ActivityData.Where(item =>
        (item.Name == activity.Name) && (item.Quantity == activity.Quantity) && (item.TransactionDate == activity.TransactionDate)
        && (item.TransactionId == activity.TransactionId)).FirstOrDefault();
            if (isExist == null)
            {
                try
                {

                    await _dbContext.ActivityData.AddAsync(activity);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }

            return activity;
        }



        public async Task<List<ActivityData>> getMobileCombustionDistanceActivityData(string organizationId, int emissionSourceId)
        {
            //todo: get only non processed data
            return await _dbContext.ActivityData
                .Include(x => x.OrganizationUnit)
                 .ThenInclude(x => x.Organization)
                .Include(x => x.ActivityType)
                .ThenInclude(x => x.EmissionsSource)
                .Where(x => x.isProcessed != true
                && x.ActivityType.Name == "Distance Activity"
                && x.ActivityType.EmissionsSource.Id == emissionSourceId
                && x.OrganizationUnit.Organization.Id.ToString() == organizationId)
                .ToListAsync();
        }

        public async Task<ActivityData> UpdateActivityData(ActivityData activity)
        {
            activity.isProcessed = true;
            _dbContext.ActivityData.Update(activity);
            await _dbContext.SaveChangesAsync();
            return activity;
        }

        public async Task<List<PurchasedEnergyData>> GetPurchasedElectricityActivityData(Guid organizationUnitId)
        {
            return await _dbContext.PurchasedEnergyData
                .Include(x => x.OrganizationUnit)
                .Include(x => x.ActivityType)
                .ThenInclude(x => x.EmissionsSource)
                .Where(x => x.isProcessed != true
                && x.ActivityType.Name == "Purchased Electricity - Location Based"
                && x.ActivityType.EmissionsSource.Name == "Purchased Electricity"
                && x.OrganizationUnit.Id == organizationUnitId)
                .ToListAsync();
        }

        public async Task<List<StationaryCombustionData>> GetStationaryCombustionActivityDataByFuelType(Guid organizationUnitId)
        {
            return await _dbContext.StationaryCombustionData
                .Include(x => x.OrganizationUnit)
                .Include(x => x.ActivityType)
                .ThenInclude(x => x.EmissionsSource)
                .Include(x => x.FuelType)
                .Where(x => x.isProcessed != true
                && x.ActivityType.Name == "Fuel Usage - Stationary Combustion"
                && x.ActivityType.EmissionsSource.Name == "Stationary Combustion"
                && x.OrganizationUnit.Id == organizationUnitId
                && x.FuelType.Name == "Natutral Gas")
                .ToListAsync();
        }

        #region Roll Forward Functionality
        public async Task<PagedResultDto<RollForwardActivityDataModel>> GetActivityDataByOrganizationAndEmissionGroup(Guid organizationId, Guid emissionGroupId, DateTime consumptionStart, DateTime consumptionEnd)
        {
            try
            {
                emissionGroupIdList = new List<Guid>();

                var units = await _dbContext.Units.ToListAsync();

                var emissionSources = await _dbContext.EmissionsSources.ToListAsync();

                var emissionGroupTreeList = await _dbContext.EmissionGroups
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

                var activitiesData = await (from emissionGroups in _dbContext.EmissionGroups.Include(x => x.Children)
                                            join activityData in _dbContext.ActivityData.Include(x => x.OrganizationUnit).Include(x => x.Unit)
                                            on
                                            emissionGroups.Id equals activityData.EmissionGroupId
                                            join purchasedProductData in _dbContext.PurchaseProductsData
                                            on
                                            activityData.Id equals purchasedProductData.Id into purchasedProductActData
                                            from purchasedProductData in purchasedProductActData.DefaultIfEmpty()
                                            join emissionFactor in _dbContext.EmissionsFactors.Include(x => x.CO2EUnit).Include(x => x.Library)
                                            on activityData.EmissionFactorId equals emissionFactor.Id into emissionFactorActData
                                            from emissionFactor in emissionFactorActData.DefaultIfEmpty()
                                            join product in _dbContext.Products
                                            on purchasedProductData.ProductId equals product.Id into purchasedProductPrdData
                                            from product in purchasedProductPrdData.DefaultIfEmpty()
                                            join productEmission in _dbContext.ProductsEmissions
                                            on product.Id equals productEmission.ProductId into prdEmissions
                                            from productEmission in prdEmissions.DefaultIfEmpty()
                                            join emission in _dbContext.Emissions
                                            on activityData.Id equals emission.ActivityDataId into actDataEmission
                                            from emission in actDataEmission.DefaultIfEmpty()

                                            join businessTravelData in _dbContext.BusinessTravelData
                                            on
                                            activityData.Id equals businessTravelData.Id into businessTravelActData
                                            from businessTravelData in businessTravelActData.DefaultIfEmpty()

                                            join employeeCommuteData in _dbContext.EmployeeCommuteData
                                            on
                                            activityData.Id equals employeeCommuteData.Id into employeeCommuteActData
                                            from employeeCommuteData in employeeCommuteActData.DefaultIfEmpty()

                                            join transportAndDistributionData in _dbContext.TransportAndDistributionData
                                            on
                                            activityData.Id equals transportAndDistributionData.Id into transportAndDistributionActData
                                            from transportAndDistributionData in transportAndDistributionActData.DefaultIfEmpty()

                                            join fugitiveEmissionsData in _dbContext.FugitiveEmissionsData
                                            on
                                            activityData.Id equals fugitiveEmissionsData.Id into fugitiveEmissionsActData
                                            from fugitiveEmissionsData in fugitiveEmissionsActData.DefaultIfEmpty()

                                            join wasteGeneratedData in _dbContext.WasteGeneratedData
                                            on
                                            activityData.Id equals wasteGeneratedData.Id into wasteGeneratedActData
                                            from wasteGeneratedData in wasteGeneratedActData.DefaultIfEmpty()

                                            join endOfLifeTreatmentData in _dbContext.EndOfLifeTreatmentData
                                            on
                                            activityData.Id equals endOfLifeTreatmentData.Id into endOfLifeTreatmentActData
                                            from endOfLifeTreatmentData in endOfLifeTreatmentActData.DefaultIfEmpty()

                                            join greenhouseGasData in _dbContext.GreenHouseGases
                                            on fugitiveEmissionsData.GreenhouseGasId equals greenhouseGasData.Id into fugitiveEmissionsGreenhouseGas
                                            from greenhouseGasData in fugitiveEmissionsGreenhouseGas.DefaultIfEmpty()

                                            join purchasedEnergyData in _dbContext.PurchasedEnergyData
                                            on
                                            activityData.Id equals purchasedEnergyData.Id into purchasedEnergyActData
                                            from purchasedEnergyData in purchasedEnergyActData.DefaultIfEmpty()

                                            join fuelAndEnergyData in _dbContext.FuelAndEnergy
                                            on
                                            activityData.Id equals fuelAndEnergyData.Id into FuelAndEnergyActData
                                            from fuelAndEnergyData in FuelAndEnergyActData.DefaultIfEmpty()

                                            where
                                            emissionGroupIdList.Contains(activityData.EmissionGroupId ?? Guid.Empty) && !activityData.IsDeleted &&
                                            (activityData.ConsumptionStart.Date >= consumptionStart.Date && activityData.ConsumptionEnd.Date <= consumptionEnd.Date)
                                            select
                                            new
                                            RollForwardActivityDataModel
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
                                                CO2e = emission.CO2E != null ? emission.CO2E : null,
                                                CO2eUnitId = emission.CO2EUnitId != null ? emission.CO2EUnitId : null,
                                                ProductId = purchasedProductData != null ? purchasedProductData.ProductId : Guid.Empty,
                                                PurchasedProductId = purchasedProductData != null ? purchasedProductData.Id : Guid.Empty, // will be same as activity data Id
                                                Status = product != null ? product.Status : 0,
                                                ProductCode = purchasedProductData != null ? purchasedProductData.ProductCode : null,
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
                                                EmissionFactorCO2e = emissionFactor != null ? emissionFactor.CO2E : (activityData.Quantity > 0 ? emission.CO2E / activityData.Quantity : null),
                                                EmissionFactorCO2eUnitId = emissionFactor != null ? (emissionFactor.CO2EUnit != null ? emissionFactor.CO2EUnit.Id : null) : null,
                                                EmissionFactorLibraryName = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Name : null) : null,
                                                Year = emissionFactor != null ? (emissionFactor.Library != null ? emissionFactor.Library.Year : null) : null,
                                                EnergyMix = purchasedEnergyData != null ? purchasedEnergyData.EnergyMix : null,
                                                SupplierOrganization = transportAndDistributionData != null ? transportAndDistributionData.SupplierOrganization : null,
                                                ActivityTypeId = activityData.ActivityTypeId,
                                                EmissionsDataQualityScore = (int)emission.EmissionsDataQualityScore,
                                                TransportType = transportAndDistributionData != null ? transportAndDistributionData.Type : null,
                                                QuantityUnitName = activityData.Unit.Name ?? null,
                                                SourceTransactionId = activityData.SourceTransactionId ?? null,
                                                FuelTypeId = fuelAndEnergyData != null ? fuelAndEnergyData.FuelTypeId : null,
                                                EnergyType = (int?)(fuelAndEnergyData != null ? fuelAndEnergyData.EnergyType : null),
                                            })
                        .ToListAsync();

                activitiesData.ForEach(activityData =>
                {
                    activityData.EmissionFactorCO2eUnitName = activityData.EmissionFactorCO2eUnitId != null ? units.Single(x => x.Id == activityData.EmissionFactorCO2eUnitId).Name : null;
                    activityData.EmissionSourceName = activityData.EmissionSourceId > 0 ? emissionSources.Single(x => x.Id == activityData.EmissionSourceId).Name : null;
                });

                var result = new PagedResultDto<RollForwardActivityDataModel>()
                {
                    Items = activitiesData,
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

        public async Task<bool> AddMobileCombustionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new MobileCombustionData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionId = activityData.TransactionSource,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString(),
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };



                await _dbContext.MobileCombustionData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Roll Forward Method: AddMobileCombustionDataAsync - Exception: {ex}");
                return false;
            }

        }


        public async Task<bool> AddPurchasedElectricityDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var purchaseElectricityModel = new PurchasedEnergyData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionId = activityData.TransactionSource,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    EnergyType = EnergyType.Electricity,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    EnergyMix = activityData.EnergyMix,
                    SourceTransactionId = activityData.Id.ToString(),
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.PurchasedEnergyData.AddAsync(purchaseElectricityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Roll Forward Method: AddPurchasedElectricityDataAsync - Exception: {ex}");
                return false;
            }

        }

        public async Task<bool> AddStationaryCombustionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                // TODO: To check if this is desired. We should maybe check based on the Unit selected.
                // The Unit can be Liters of.... In that case we should select appropiate Fuel type.
                var fuelType = _dbContext.FuelTypes.Where(x => x.Name == "Natural Gas").FirstOrDefault();
                var stationaryCombustionModel = new StationaryCombustionData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionId = activityData.TransactionSource,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    FuelTypeId = fuelType?.Id,
                    isProcessed = false,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()

                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };


                await _dbContext.StationaryCombustionData.AddAsync(stationaryCombustionModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Roll Forward Method: AddStationaryCombustionDataAsync - Exception: {ex}");
                return false;
            }

        }

        public async Task<bool> AddBusinessTravelDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new BusinessTravelData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = (Guid)activityData.VehicleTypeId,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.BusinessTravelData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddBusinessTravelDataAsync - Exception: {exception}");
                return false;
            }
        }


        public async Task<bool> AddEmployeeCommuteDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new EmployeeCommuteData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = (Guid)activityData.VehicleTypeId,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.EmployeeCommuteData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddEmployeeCommuteDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddEndOfLifeTreatmentDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new EndOfLifeTreatmentData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    WasteTreatmentMethod = (int)activityData.WasteTreatmentMethod,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.EndOfLifeTreatmentData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddEndOfLifeTreatmentDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddFugitiveEmissionsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new FugitiveEmissionsData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    GreenhouseGasId = (Guid)activityData.GreenhouseGasId,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.FugitiveEmissionsData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddFugitiveEmissionsDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddPurchasedProductsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new PurchasedProductsData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionId = activityData.TransactionId,
                    // TODO: To be revised and adjusted in case that from the front end this will be something that the user can set
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    ProductId = activityData.ProductId,
                    ProductCode = activityData.ProductCode,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = activityData.ProductId,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.Product,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CO2eFactor = activityData.ProductCO2eq,
                    CO2eFactorUnitId = activityData.ProductCO2eqUnitId,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.PurchaseProductsData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ProductId = activityData.ProductId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddPurchasedProductsDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddTransportAndDistributionDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new TransportAndDistributionData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    VehicleTypeId = activityData.VehicleTypeId ?? Guid.Empty,
                    SupplierOrganization = activityData.SupplierOrganization,
                    Type = (int)activityData.TransportType,
                    GoodsQuantity = activityData.GoodsQuantity,
                    GoodsUnitId = activityData.GoodsUnitId,
                    Distance = activityData.Distance,
                    DistanceUnitId = activityData.DistanceUnitId,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.TransportAndDistributionData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddTransportAndDistributionDataAsync - Exception: {exception}");
                return false;
            }
        }


        public async Task<bool> AddWasteGeneratedDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new WasteGeneratedData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    WasteTreatmentMethod = (int)activityData.WasteTreatmentMethod,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.WasteGeneratedData.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddWasteGeneratedDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddUseOfSoldProductsDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new UseOfSoldProductsData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString()
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.UseOfSoldProducts.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddUseOfSoldProductsDataAsync - Exception: {exception}");
                return false;
            }
        }

        public async Task<bool> AddFuelAndEnergyDataAsync(RollForwardActivityDataModel activityData, DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime targetPeriodEnd)
        {
            try
            {
                var activityId = Guid.NewGuid();
                var updatedDescription = ActivityDataDescriptionCreationHelper.CreateActivityDataDescription(activityData.Quantity, activityData.QuantityUnitName, activityData.CO2e);
                var updatedConsumptionStart = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item1;
                var updatedConsumptionEnd = RollForwardConsumptionDateUpdateHelper.UpdateConsumptionDates(sourcePeriodStart, targetPeriodStart, (DateTime)activityData.ConsumptionStart, (DateTime)activityData.ConsumptionEnd).Item2;

                var conversionFactorsList = _dbContext.ConversionFactors.Where(x => x.ActivityDataId == activityData.Id);

                var activityModel = new FuelAndEnergyData
                {
                    Id = activityId,
                    Name = activityData.Name,
                    Quantity = 0,
                    UnitId = activityData.UnitId,
                    TransactionDate = updatedConsumptionEnd,
                    ActivityTypeId = activityData.ActivityTypeId,
                    Description = updatedDescription,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = updatedConsumptionStart,
                    ConsumptionEnd = updatedConsumptionEnd,
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    isProcessed = false,
                    EmissionGroupId = activityData.EmissionGroupId,
                    EmissionFactorId = activityData.EmissionFactorId,
                    SourceTransactionId = activityData.Id.ToString(),
                    FuelTypeId = activityData.FuelTypeId,
                    EnergyType = (EnergyType?)activityData.EnergyType
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = activityData.OrganizationUnitId,
                    EmissionsDataQualityScore = (EmissionsDataQualityScore?)activityData.EmissionsDataQualityScore,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = Guid.Empty,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.User,
                    IsActive = true,
                    CO2E = null,
                    CO2EUnitId = null,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                await _dbContext.FuelAndEnergy.AddAsync(activityModel);
                await _dbContext.Emissions.AddAsync(emissionsModel);
                await _dbContext.SaveChangesAsync();

                if (conversionFactorsList.Any())
                {
                    var newFactors = conversionFactorsList.Select(factor => new ConversionFactors
                    {
                        ActivityDataId = activityId,
                        ConversionFactor = factor.ConversionFactor,
                        ConversionUnit = factor.ConversionUnit
                    }).ToList();

                    _dbContext.ConversionFactors.AddRange(newFactors);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Method: Roll Forward AddFuelAndEnergyDataAsync - Exception: {exception}");
                return false;
            }
        }

        #endregion

    }
}
