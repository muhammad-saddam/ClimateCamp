using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.Services;

namespace ClimateCamp.GHG.Calculations
{
    public  class RydooMobileCombustionServiceBusTriggeredFunction
    {

        private readonly IMobileCombustionCalculationService _calculationService;
        public RydooMobileCombustionServiceBusTriggeredFunction(IMobileCombustionCalculationService calculationService)
        {
            _calculationService = calculationService;
        }
        [FunctionName(nameof(RydooMobileCombustionServiceBusTriggeredFunction))]
        public async Task Run([ServiceBusTrigger("%CalculationInitiatorQueueName%", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            try
            {
               // var result = await _calculationService.SaveGHGEmissions(log);
                log.LogInformation($"MobileCombustionCalculationFunction - _calculationService - SaveGHGEmissions response: {true}");
            }
            catch (Exception ex)
            {
                log.LogInformation($"Exception: {ex.InnerException}");
            }
        }
    }
}
