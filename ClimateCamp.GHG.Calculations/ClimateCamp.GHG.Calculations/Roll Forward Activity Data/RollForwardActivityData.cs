using ClimateCamp.GHG.Calculations.Services.RollForwardActivityData;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations
{
    public class RollForwardActivityData
    {
        private readonly IRollForwardActivityData _rollForwardActivityData;
        private readonly ILogger _logger;

        public RollForwardActivityData(IRollForwardActivityData rollForwardActivityData, ILoggerFactory loggerFactory)
        {
            _rollForwardActivityData = rollForwardActivityData;
            _logger = loggerFactory.CreateLogger<RollForwardActivityData>();

        }

        [Function("RollForwardActivityData")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req
            )
        {
            _logger.LogInformation($"RollForwardActivityData - Function Executing!");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            Guid organizationId = (Guid)data?.OrganizationId;
            DateTime consumptionStart = data?.ConsumptionStart;
            DateTime consumptionEnd = data?.ConsumptionEnd;
            DateTime targetPeriodStart = data?.TargetPeriodStart;
            DateTime targetPeriodEnd = data?.TargetPeriodEnd;

            var response = await _rollForwardActivityData.RollForwardActivityDataByOrganizationId(organizationId, consumptionStart, consumptionEnd, targetPeriodStart, targetPeriodEnd);
            
            var responseData = req.CreateResponse(HttpStatusCode.OK);
            await responseData.WriteAsJsonAsync(response);

            return responseData;
        }
    }
}
