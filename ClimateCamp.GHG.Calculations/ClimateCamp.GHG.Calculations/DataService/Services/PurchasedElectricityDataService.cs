using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.GHG.Calculations.DataService.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Services
{
    public class PurchasedEnergyDataService : IPurchasedEnergyDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger<PurchasedEnergyDataService> _logger;
        public PurchasedEnergyDataService(CommonDbContext dbContext, ILogger<PurchasedEnergyDataService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<PurchasedEnergyData>> GetPurchasedElectricityActivityData(string organizationId, int emissionSourceId)
        {
            try
            {
                return await _dbContext.PurchasedEnergyData
                    .Include(x => x.OrganizationUnit)
                    .ThenInclude(x => x.Organization)
                    .Include(x => x.ActivityType)
                    .ThenInclude(x => x.EmissionsSource)
                    .Where(x => !x.isProcessed && x.ActivityType.EmissionsSource.Id == emissionSourceId
                    && x.OrganizationUnit.Organization.Id.ToString() == organizationId
                    && !x.isProcessed)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Method: GetPurchasedElectricityActivityData - Exception: {ex}");
                return null;
            }

        }
    }
}
