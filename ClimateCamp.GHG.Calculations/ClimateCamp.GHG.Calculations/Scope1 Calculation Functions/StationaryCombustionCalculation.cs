using Calculations.Services.StationaryCombustionCalculation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Scope1_Calculation_Functions
{

    public class StationaryCombustionCalculation
    {

        private readonly IStationaryCombustionCalculationService _stationaryCombustionCalculationService;
        private readonly ILogger _logger;
        public StationaryCombustionCalculation(ILoggerFactory logger, IStationaryCombustionCalculationService stationaryCombustionCalculationService)
        {
            _stationaryCombustionCalculationService = stationaryCombustionCalculationService;
            _logger = logger.CreateLogger<StationaryCombustionCalculation>();
        }
        [Function("StationaryCombustionCalculation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation($"StationaryCombustionCalculation - Executing!");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string organizationId = data?.OrganizationId;
            int emissionSourceId = data?.EmissionSourceId != null ? Convert.ToInt32(data?.EmissionSourceId) : 0;
            var result = await _stationaryCombustionCalculationService.SaveGHGEmissions(_logger, organizationId, emissionSourceId);
            _logger.LogInformation($"StationaryCombustionCalculation - Execute with result: {result}!");
            return new OkObjectResult(result);
        }
    }
}
