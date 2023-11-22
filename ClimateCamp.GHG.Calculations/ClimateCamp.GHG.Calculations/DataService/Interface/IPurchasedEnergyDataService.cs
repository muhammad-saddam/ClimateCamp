using ClimateCamp.CarbonCompute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Interface
{
    public interface IPurchasedEnergyDataService
    {
        Task<List<PurchasedEnergyData>> GetPurchasedElectricityActivityData(string organizationId, int emissionSourceId);
    }
}