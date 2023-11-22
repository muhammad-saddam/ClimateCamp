using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Calculations.Services.StationaryCombustionCalculation
{
    public interface IStationaryCombustionCalculationService
    {
        Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId);
    }
}
