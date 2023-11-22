using ClimateCamp.CarbonCompute;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public interface IEmissionDataService
    {
        Task<int> SaveEmissions(Emission emission);
    }
}
