using ClimateCamp.CarbonCompute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Interface
{
    public interface IStationaryCombustionDataService
    {
        Task<List<StationaryCombustionData>> GetStationaryCombustionActivityData(string organizationId, int emissionSourceId);
    }
}
