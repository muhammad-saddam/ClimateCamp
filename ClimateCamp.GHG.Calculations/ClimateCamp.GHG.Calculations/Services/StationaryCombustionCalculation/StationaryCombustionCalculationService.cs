
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

namespace Calculations.Services.StationaryCombustionCalculation
{
    public class StationaryCombustionCalculationService : IStationaryCombustionCalculationService
    {
        private readonly IGreenhouseGasesDataService _greenhouseGasesDataService;
        private readonly IStationaryCombustionDataService _stationaryCombustionCalculationDateService;
        private readonly IEmissionDataService _emissionDataService;
        private readonly IEmissionsFactorsDataService _emissionsFactorsDataService;
        private readonly IActivityDataService _activityDataService;
        private readonly CommonDbContext _dbContext;
        public StationaryCombustionCalculationService(
            IGreenhouseGasesDataService greenhouseGasesDataService,
            IStationaryCombustionDataService stationaryCombustionCalculationDateService,
            IEmissionDataService emissionDataService,
            IEmissionsFactorsDataService emissionsFactorsDataService,
            IActivityDataService activityDataService,
            CommonDbContext dbContext)
        {
            _greenhouseGasesDataService = greenhouseGasesDataService;
            _stationaryCombustionCalculationDateService = stationaryCombustionCalculationDateService;
            _emissionDataService = emissionDataService;
            _emissionsFactorsDataService = emissionsFactorsDataService;
            _activityDataService = activityDataService;
            _dbContext = dbContext;
        }

        public async Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId)
        {
            try
            {
                log.LogInformation($" Class : {0} , Method: {1} Executing", "StationaryCombustionCalculationService", "SaveGHGEmissions");
                var activityDataList = await _stationaryCombustionCalculationDateService.GetStationaryCombustionActivityData(organizationId, emissionSourceId);
                var units = _dbContext.Units.ToList();
                var organization = _dbContext.Organizations.Where(x => x.Id == Guid.Parse(organizationId)).FirstOrDefault();
                var gasesData = await _greenhouseGasesDataService.GetGreenHouseGasesList();
                //this need to be dynamic like no need to check which gas just iterate through the list and do calculations based upon gas name or Id
                //get ghg gases and their GWP factors
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
                    //get emission Factor by emission sourceID and unit id
                    // add library id
                    var emisionFactor = await _emissionsFactorsDataService.GetEmissionFactorsByEmissionSourceUnitId(emissionSourceId, activity.UnitId.Value, organization.EmissionsFactorsLibraryId.ToString());
                    if (emisionFactor != null)
                    {
                        var emisionFactorModel = new EmissionFactorModel()
                        {
                            Co2 = emisionFactor.CO2,
                            Co2unitId = emisionFactor.CO2Unit?.Id,
                            Ch4 = emisionFactor.CH4,
                            Ch4unitId = emisionFactor.CH4Unit?.Id,
                            N20 = emisionFactor.N20,
                            N20unitId = emisionFactor.N20Unit?.Id,
                            CO2e = emisionFactor.CO2E,
                            Co2eunitId = emisionFactor.CO2EUnit?.Id,
                        };
                        //Calculate StationaryMobileCombustion emissions
                        Emission emission = new Emission();
                        emission.CO2 = (float)GHGCalculationHelper.CalculateStationaryCombustionCO2EmissionFactors(activity.Quantity, emisionFactorModel);
                        emission.CO2Unit = units.Where(x => x.Id == emisionFactorModel.Co2unitId).FirstOrDefault();
                        emission.CH4 = (float)GHGCalculationHelper.CalculateStationaryCombustionCH4EmissionFactors(activity.Quantity, emisionFactorModel);
                        emission.CH4Unit = units.Where(x => x.Id == emisionFactorModel.Ch4unitId).FirstOrDefault();
                        emission.N20 = (float)GHGCalculationHelper.CalculateStationaryCombustionN20EmissionFactors(activity.Quantity, emisionFactorModel);
                        emission.N20Unit = units.Where(x => x.Id == emisionFactorModel.N20unitId).FirstOrDefault();

                        //check if CO2E 1 = 0 and other = 0 than conside CO2E else use all others
                        if (emisionFactorModel.CO2e != 0 && (emisionFactorModel.Co2 == 0 || emisionFactorModel.N20 == 0 || emisionFactorModel.Ch4 == 0))
                        {
                            emission.CO2E = activity.Quantity * emisionFactorModel.CO2e;
                        }
                        else
                        {
                            emission.CO2E = (float)GHGCalculationHelper.CalculateCO2eEmissionFactors(emission, gwpFactors);
                        }
                        emission.CO2EUnit = units.Where(x => x.Id == emisionFactorModel.Co2eunitId.Value).FirstOrDefault();
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
                log.LogInformation($" Class : {0} , Method: {1} Executed Succesfully!", "StationaryCombustionCalculationService", "SaveGHGEmissions");
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
