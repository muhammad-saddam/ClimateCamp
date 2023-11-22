using Microsoft.Extensions.Logging;
using Mobile.Combustion.Calculation.Models;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services
{
    public interface IMobileCombustionCalculationService
    {
        Task<int> ReadFileAndSaveData(RequestModel request, ILogger log);
        Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId);
    }
}
