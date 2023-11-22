using ClimateCamp.CarbonCompute;
using ClimateCamp.GHG.Calculations.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService
{
    public interface IProductEmissionsDataService
    {
        Task<ProductEmissions> GetProductFootprintData(Guid productId);
    }
}
