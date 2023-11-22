using ClimateCamp.GHG.Calculations.Services.PathfinderApi;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Pathfinder
{
    public class PathfinderApi
    {
        private readonly ILogger _logger;
        private readonly IPathfinderApi _pathfinderApi;

        public PathfinderApi(ILoggerFactory loggerFactory, IPathfinderApi pathfinderApi)
        {
            _logger = loggerFactory.CreateLogger<PathfinderApi>();
            _pathfinderApi = pathfinderApi;
        }

        [Function("PathfinderApi")]
        public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "2/footprints/{id}")] HttpRequestData req, Guid id)
        {
            _logger.LogInformation($"PathfinderApi - GetPcf - Function Executing");

            var productFootprint = await _pathfinderApi.CreatePathfinderPcfObject(id);

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(productFootprint, options);

            var responseData = req.CreateResponse(HttpStatusCode.OK);
            responseData.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await responseData.WriteStringAsync(json);

            return responseData;
        }

    }
}
