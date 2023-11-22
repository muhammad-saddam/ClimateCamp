using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Services
{
    public class ProductEmissionsDataService : IProductEmissionsDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;

        public ProductEmissionsDataService(ILoggerFactory loggerFactory, CommonDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<ProductEmissionsDataService>();
        }

        public async Task<ProductEmissions> GetProductFootprintData(Guid productId)
        {
            var pcfData = await _dbContext.ProductsEmissions
            .Include(x => x.ProductsEmissionSources)
            .Include(x => x.Product)
            .ThenInclude(x => x.Organization)
            .Where(x => x.ProductId == productId && x.EmissionSourceType == 2)
            .FirstOrDefaultAsync();

            return pcfData;
        }
    }
}
