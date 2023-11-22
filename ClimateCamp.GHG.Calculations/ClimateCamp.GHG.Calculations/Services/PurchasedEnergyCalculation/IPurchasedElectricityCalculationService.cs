using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Purchased.Energy.Calculation.Services
{
    public interface IPurchasedElectricityCalculationService
    {
        Task<bool> SaveGHGEmissions(ILogger log, string organizationId, int emissionSourceId);
    }
}
