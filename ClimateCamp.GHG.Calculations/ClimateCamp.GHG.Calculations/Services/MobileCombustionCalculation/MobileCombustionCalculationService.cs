using Calculation.Helpers;
using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.DataService;
using Mobile.Combustion.Calculation.Models;
using Mobile.Combustion.Calculation.Services.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services
{

    public class MobileCombustionCalculationService : IMobileCombustionCalculationService
    {
        private readonly IBlobFileService _blobFileService;
        private readonly string containerName = "";
        /// private readonly string fileName = "mileage-report.xlsx";
        private readonly ILogger _logger;
        private readonly IGreenhouseGasesDataService _greenhouseGasesDataService;
        private readonly IActivityDataService _activityDataService;
        private readonly IEmissionDataService _emissionDataService;
        private readonly IEmissionsFactorsDataService _emissionsFactorsDataService;
        private readonly IQueueMessageService _queueMessageService;
        private readonly CommonDbContext _dbContext;
        public MobileCombustionCalculationService(
            IBlobFileService blobFileService,
            IGreenhouseGasesDataService greenhouseGasesDataService,
            IActivityDataService activityDataService,
            IQueueMessageService queueMessageService,
            IEmissionDataService emissionDataService,
            IEmissionsFactorsDataService emissionsFactorsDataService,
            CommonDbContext dbContext)
        {
            _blobFileService = blobFileService;
            _greenhouseGasesDataService = greenhouseGasesDataService;
            _activityDataService = activityDataService;
            _emissionDataService = emissionDataService;
            _queueMessageService = queueMessageService;
            _emissionsFactorsDataService = emissionsFactorsDataService;
            _dbContext = dbContext;
        }

        public async Task<int> ReadFileAndSaveData(RequestModel request, ILogger log)
        {
            log.LogInformation($"Method: ReadFileAndSaveData Executing");
            var activityList = new List<ActivityData>();
            try
            {
                var fileModel = new ReadFileModel()
                {
                    BlobContainerName = request.rootPath,
                    FileName = request.fileName
                };
                //todo: confirm it to use
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = await _blobFileService.GetFileStream(fileModel))
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                    int rowCount = worksheet.Dimension.End.Row;     //get row count                  
                    for (int row = 2; row <= rowCount; row++)
                    {



                        var organizationUnitId = _dbContext.OrganizationUnits
                            .Include(x => x.Organization)
                            .Where(y => y.Organization.Name == request.organizationName.ToString()
                        && y.Name == worksheet.Cells[row, 5].Value.ToString()).Select(x => x.Id).FirstOrDefault();

                        var activityModel = new ActivityData()
                        {
                            Id = Guid.NewGuid(),
                            Name = worksheet.Cells[row, 2].Value.ToString().Trim() + "-" + worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Quantity = float.Parse((worksheet.Cells[row, 10].Value.ToString())),
                            UnitId = (int)GHG.UnitsEnum.km,
                            TransactionId = worksheet.Cells[row, 7].Value.ToString(),
                            TransactionDate = Convert.ToDateTime(worksheet.Cells[row, 11].Value),
                            ActivityTypeId = (int)GHG.ActivityTypeEnum.DistanceActivity,
                            Description = "Vehicle Trip: From " + worksheet.Cells[row, 8].Value.ToString() + " - To: " + worksheet.Cells[row, 9].Value.ToString(),
                            IsActive = true,
                            //todo: confirm
                            DataQualityType = GHG.DataQualityType.Actual,
                            IndustrialProcessId = 1,
                            IsDeleted = false,
                            ConsumptionStart = DateTime.UtcNow,
                            ConsumptionEnd = DateTime.UtcNow,
                            CreationTime = DateTime.UtcNow,
                            OrganizationUnitId = organizationUnitId,
                        };
                        activityList.Add(activityModel);
                    }
                }
                foreach (var activity in activityList)
                {
                    await _activityDataService.SaveActivityData(activity);
                }

                await _queueMessageService.PushMessageToQueue(new ServiceBusMessageModel()
                {
                    Content = "Initiate-Execution",
                    Id = Convert.ToString(Guid.NewGuid()),
                    Type = "",
                    QueueName = Environment.GetEnvironmentVariable("CalculationInitiatorQueueName")
                });
                log.LogInformation($"Method: ReadFileAndSaveData Executed");
                return activityList.Count;
            }
            catch (Exception ex)
            {
                log.LogInformation($"Method: SaveGHGEmissions - Exception: {ex}");
                throw;
            }
        }


        public async Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId)
        {
            try
            {
                log.LogInformation($"Method: SaveGHGEmissions Executing");
                var activityDataList = await _activityDataService.getMobileCombustionDistanceActivityData(organizationId, emissionSourceId);
                var units = _dbContext.Units.ToList();
                var organization = _dbContext.Organizations.FirstOrDefault(x => x.Id.ToString() == organizationId);
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

                    var emisionFactor = await _emissionsFactorsDataService.GetEmissionFactorsByEmissionSourceUnitId(emissionSourceId, activity.UnitId.Value, organization.EmissionsFactorsLibraryId.ToString());
                    var emisionFactorModel = new EmissionFactorModel()
                    {
                        Co2 = emisionFactor.CO2,
                        Co2unitId = emisionFactor.CO2Unit.Id,
                        Ch4 = emisionFactor.CH4,
                        Ch4unitId = emisionFactor.CH4Unit.Id,
                        N20 = emisionFactor.N20,
                        N20unitId = emisionFactor.N20Unit.Id,
                        Co2eunitId = emisionFactor.CO2EUnit.Id,
                    };
                    Emission emission = new Emission();
                    emission.CO2 = (float)GHGCalculationHelper.CalculateCO2EmissionFactors(activity.Quantity, emisionFactorModel);
                    emission.CO2Unit = units.FirstOrDefault(x => x.Id == emisionFactorModel.Co2unitId.Value);
                    emission.CH4 = (float)GHGCalculationHelper.CalculateCH4EmissionFactors(activity.Quantity, emisionFactorModel);
                    emission.CH4Unit = units.FirstOrDefault(x => x.Id == emisionFactorModel.Ch4unitId.Value);
                    emission.N20 = (float)GHGCalculationHelper.CalculateN20EmissionFactors(activity.Quantity, emisionFactorModel);
                    emission.N20Unit = units.FirstOrDefault(x => x.Id == emisionFactorModel.N20unitId.Value);
                    //check if CO2E 1 = 0 and other = 0 than conside CO2E else use all others
                    if (emisionFactorModel.CO2e != 0 && (emisionFactorModel.Co2 == 0 || emisionFactorModel.N20 == 0 || emisionFactorModel.Ch4 == 0))
                    {
                        emission.CO2E = activity.Quantity * emisionFactorModel.CO2e;
                    }
                    else
                    {
                        emission.CO2E = (float)GHGCalculationHelper.CalculateCO2eEmissionFactors(emission, gwpFactors);
                    }
                    emission.CO2EUnit = units.FirstOrDefault(x => x.Id == emisionFactorModel.Co2eunitId.Value);
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
                log.LogInformation($"Method: SaveGHGEmissions Executed");
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
