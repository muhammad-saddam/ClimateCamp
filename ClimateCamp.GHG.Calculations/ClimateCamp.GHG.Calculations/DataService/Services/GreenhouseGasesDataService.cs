using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public class GreenhouseGasesDataService : IGreenhouseGasesDataService
    {
        private readonly CommonDbContext _dbContext;
        public GreenhouseGasesDataService(CommonDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<GreenhouseGas>> GetGreenHouseGasesList()
        {
            return await _dbContext.GreenHouseGases.ToListAsync();
        }
    }
}
