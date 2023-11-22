using ClimateCamp.GHG.Calculations.Pathfinder;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.PathfinderApi
{
    public interface IPathfinderApi
    {
        Task<ProductFootprintResponse> CreatePathfinderPcfObject(Guid productId);

    }
}
