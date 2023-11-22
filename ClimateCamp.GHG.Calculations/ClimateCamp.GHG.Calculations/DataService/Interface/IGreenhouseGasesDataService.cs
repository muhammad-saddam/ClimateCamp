using ClimateCamp.CarbonCompute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public interface IGreenhouseGasesDataService
    {
        Task<List<GreenhouseGas>> GetGreenHouseGasesList();
    }
}
