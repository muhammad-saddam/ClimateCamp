using Calculation.Helpers;
using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.DataService.Interface;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.DataService;
using Mobile.Combustion.Calculation.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Purchased.Energy.Calculation.Services
{
    public class PurchasedElectricityCalculationService : IPurchasedElectricityCalculationService
    {
        private readonly IGreenhouseGasesDataService _greenhouseGasesDataService;
        private readonly IActivityDataService _activityDataService;
        private readonly IEmissionDataService _emissionDataService;
        private readonly IEmissionsFactorsDataService _emissionsFactorsDataService;
        private readonly IPurchasedEnergyDataService _purchasedEnergyCalculationDataService;
        private readonly CommonDbContext _dbContext;

        public PurchasedElectricityCalculationService(
            IGreenhouseGasesDataService greenhouseGasesDataService,
            IActivityDataService activityDataService,
            IEmissionDataService emissionDataService,
            IEmissionsFactorsDataService emissionsFactorsDataService,
            IPurchasedEnergyDataService purchasedEnergyCalculationDataService,
            CommonDbContext dbContext)
        {
            _greenhouseGasesDataService = greenhouseGasesDataService;
            _activityDataService = activityDataService;
            _emissionDataService = emissionDataService;
            _emissionsFactorsDataService = emissionsFactorsDataService;
            _purchasedEnergyCalculationDataService = purchasedEnergyCalculationDataService;
            _dbContext = dbContext;
        }

        public async Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId)
        {
            try
            {
                log.LogInformation($" Class : {0} , Method: {1} Executing", "PurchasedElectricityCalculationService", "SaveGHGEmissions");
                var activityDataList = await _purchasedEnergyCalculationDataService.GetPurchasedElectricityActivityData(organizationId, emissionSourceId);
                var units = _dbContext.Units.ToList();
                var organization = _dbContext.Organizations.Where(x => x.Id == Guid.Parse(organizationId)).FirstOrDefault();
                var gasesData = await _greenhouseGasesDataService.GetGreenHouseGasesList();
                //this need to be dynamic like no need to check which gas just iterate through the list and do calculations based upon gas name or Id
                var CO2Reading = gasesData.FirstOrDefault(t => t.Code == GHG.GreenhouseGasesCodeEnum.CO2.ToString());
                var CH4Reading = gasesData.FirstOrDefault(t => t.Code == GHG.GreenhouseGasesCodeEnum.CH4.ToString());
                var N2OReading = gasesData.FirstOrDefault(t => t.Code == GHG.GreenhouseGasesCodeEnum.N2O.ToString());
                var gwpFactors = new CalculateCO2eEmissionFactorModel()
                {
                    CO2 = CO2Reading.GWPFactor,
                    CH4 = CH4Reading.GWPFactor,
                    N2O = N2OReading.GWPFactor,
                };

                foreach (var activity in activityDataList)
                {
                    // get emission Factor by emission sourceID and unit id
                    // add library id
                    var emissionFactor = await _emissionsFactorsDataService.GetEmissionFactorsByEmissionSourceUnitId(emissionSourceId, activity.UnitId.Value, organization.EmissionsFactorsLibraryId.ToString());
                    if (emissionFactor != null)
                    {
                        var emissionFactorModel = new EmissionFactorModel()
                        {
                            Co2 = emissionFactor.CO2,
                            Co2unitId = emissionFactor.CO2Unit?.Id,
                            Ch4 = emissionFactor.CH4,
                            Ch4unitId = emissionFactor.CH4Unit?.Id,
                            N20 = emissionFactor.N20,
                            N20unitId = emissionFactor.N20Unit?.Id,
                            CO2e = emissionFactor.CO2E,
                            Co2eunitId = emissionFactor.CO2EUnit?.Id,
                        };
                        //Calculate Purchased Electricity emissions
                        var emission = new Emission()
                        {
                            CO2Unit = units.FirstOrDefault(x => x.Id == emissionFactorModel.Co2unitId),
                            CH4Unit = units.FirstOrDefault(x => x.Id == emissionFactorModel.Ch4unitId),
                            N20Unit = units.FirstOrDefault(x => x.Id == emissionFactorModel.N20unitId)
                        };

                        if (!string.Equals(activity.ActivityType.Name, "Solar Panels Generated Electricity", StringComparison.OrdinalIgnoreCase))

                        {
                            emission.CO2 = (float)GHGCalculationHelper.CalculatePurchasedElectricityCO2EmissionFactors(activity.Quantity, emissionFactorModel);
                            emission.CH4 = (float)GHGCalculationHelper.CalculatePurchasedElectricityCH4EmissionFactors(activity.Quantity, emissionFactorModel);
                            emission.N20 = (float)GHGCalculationHelper.CalculatePurchasedElectricityN20EmissionFactors(activity.Quantity, emissionFactorModel);
                            //check if CO2E 1 = 0 and other = 0 than conside CO2E else use all others
                            if (emissionFactorModel.CO2e != 0 && (emissionFactorModel.Co2 == 0 || emissionFactorModel.N20 == 0 || emissionFactorModel.Ch4 == 0))
                            {
                                emission.CO2E = activity.Quantity * emissionFactorModel.CO2e;
                            }
                            else
                            {
                                emission.CO2E = (float)GHGCalculationHelper.CalculateCO2eEmissionFactorsDynamically(emission, gwpFactors);
                            }
                        }
                        //if ActivityType.Name is Solar Panels Generated Electricity, assign value 0 to all emission factors
                        else
                        {
                            emission.CO2 = 0;
                            emission.CH4 = 0;
                            emission.N20 = 0;
                            emission.CO2E = 0;
                        }

                        emission.CO2EUnit = units.FirstOrDefault(x => x.Id == emissionFactorModel.Co2eunitId.Value);
                        emission.OrganizationUnitId = activity.OrganizationUnitId;
                        emission.EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Averaged;
                        emission.ActivityDataId = activity.Id;
                        emission.CreationTime = DateTime.UtcNow;
                        //static at the moment need to do dynamically
                        emission.EmissionsFactorsLibraryId = organization.EmissionsFactorsLibraryId != null ? organization.EmissionsFactorsLibraryId.Value : _dbContext.EmissionsFactorsLibrary.FirstOrDefault().Id;
                        var result = await _emissionDataService.SaveEmissions(emission);
                        if (result != 0)
                            await _activityDataService.UpdateActivityData(activity);
                    }
                }
                log.LogInformation($" Class : {0} , Method: {1} Executed Succesfully!", "PurchasedElectricityCalculationService", "SaveGHGEmissions");
                return true;
            }

            catch (Exception ex)
            {
                log.LogInformation($"Method: SaveGHGEmissions - Exception: {ex}");
                return false;
            }
        }
    }
}
