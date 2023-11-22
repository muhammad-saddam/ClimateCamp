using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.Services;
using System.IO;
using Newtonsoft.Json;
using Mobile.Combustion.Calculation.Models;

namespace Mobile.Combustion.Calculation
{
    public class SaveMobilityExcelDataFunction
    {
        private readonly IMobileCombustionCalculationService _calculationService;
        public SaveMobilityExcelDataFunction(IMobileCombustionCalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [FunctionName(nameof(SaveMobilityExcelDataFunction))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            RequestModel request = new RequestModel();
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            request.fileName = data?.blobName;
            request.rootPath = data?.folderPath;
            request.organizationName = data?.organizationName;
            var totalActivities = await _calculationService.ReadFileAndSaveData(request, log);
            log.LogInformation($"SaveMobilityExcelDataFunction - _calculationService - ReadFileAndSaveData response: {totalActivities}");

            return new OkObjectResult(totalActivities);
        }
    }
}
