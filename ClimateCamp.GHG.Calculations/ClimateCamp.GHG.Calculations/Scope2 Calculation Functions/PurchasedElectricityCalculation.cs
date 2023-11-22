
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Purchased.Energy.Calculation.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations
{
    public class PurchasedElectricityCalculation
    {
        private readonly IPurchasedElectricityCalculationService _purchasedElectricityCalculationService;
        private readonly ILogger _logger;
        public PurchasedElectricityCalculation(ILoggerFactory logger, IPurchasedElectricityCalculationService purchasedElectricityCalculationService)
        {
            _purchasedElectricityCalculationService = purchasedElectricityCalculationService;
            _logger = logger.CreateLogger<PurchasedElectricityCalculation>();
        }

        [Function("PurchasedElectricityCalculation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation($"PurchasedElectricityCalculation - Executing!");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string organizationId = data?.OrganizationId;
            int emissionSourceId = data?.EmissionSourceId != null ? Convert.ToInt32(data?.EmissionSourceId) : 0;
            var result = await _purchasedElectricityCalculationService.SaveGHGEmissions(_logger, organizationId, emissionSourceId);
            _logger.LogInformation($"PurchasedElectricityCalculation - Execute with result: {result}!");
            return new OkObjectResult(result);
        }
    }
}

