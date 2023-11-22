using ClimateCamp.GHG.Calculations.Services.GenericEmissionCalculation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace ClimateCamp.GHG.Calculations
{
    public class GenericEmissionCaculation
    {
        private readonly IGenericEmissionCalculation _genericEmissionCaculation;
        private readonly ILogger _logger;
        public GenericEmissionCaculation(ILoggerFactory loggerFactory, IGenericEmissionCalculation genericEmissionCaculation)
        {
            _genericEmissionCaculation = genericEmissionCaculation;
            _logger = loggerFactory.CreateLogger<GenericEmissionCaculation>();
        }



        [Function("GenericEmissionCaculation")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req
            )
        {
            _logger.LogInformation($"GenericEmissionCaculation - Function Executing!");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                var emissionFactorId = (Guid)data?.EmissionFactorId;
                var quantity = (float)data?.Quantity;
                var unitId = (int)data?.UnitId;
                var userConversionFactor = (float)data?.UserConversionFactor;
                var productId = (Guid)data?.ProductId;
                var response = await _genericEmissionCaculation.CalculateGenericEmission(emissionFactorId, quantity, unitId, userConversionFactor, productId);
                var responseData = req.CreateResponse(HttpStatusCode.OK);
                await responseData.WriteAsJsonAsync(response);
                return responseData;
            }
            catch(Exception ex)
            {
                _logger.LogError($"{nameof(Run)} = Error: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
