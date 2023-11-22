using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations
{
    public class MobileCombustionCalculation
    {
        private readonly IMobileCombustionCalculationService _calculationService;
        private readonly ILogger _logger;
        public MobileCombustionCalculation(ILoggerFactory logger,  IMobileCombustionCalculationService calculationService)
        {
            _calculationService = calculationService;
            _logger = logger.CreateLogger<MobileCombustionCalculation>();
        }

        [Function("DistanceActivityCalculation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation($"DistanceActivityCalculation - Function Executing!");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string organizationId = data?.OrganizationId;
            int emissionSourceId = data?.EmissionSourceId != null ? Convert.ToInt32(data?.EmissionSourceId) : 0;
            var result = await _calculationService.SaveGHGEmissions(_logger, organizationId, emissionSourceId);
            _logger.LogInformation($"DistanceActivityCalculation - _calculationService - SaveGHGEmissions response: {true}");

            return new OkObjectResult(result);
        }
    }
}
