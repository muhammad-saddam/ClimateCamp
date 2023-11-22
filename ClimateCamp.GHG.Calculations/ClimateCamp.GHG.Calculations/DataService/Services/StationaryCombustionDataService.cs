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
    public class StationaryCombustionDataService : IStationaryCombustionDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger<StationaryCombustionDataService> _logger;
        public StationaryCombustionDataService(CommonDbContext dbContext, ILogger<StationaryCombustionDataService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<StationaryCombustionData>> GetStationaryCombustionActivityData(string organizationId, int emissionSourceId)
        {
            try
            {
                return await _dbContext.StationaryCombustionData
                    .Include(x => x.OrganizationUnit)
                    .ThenInclude(x => x.Organization)
                    .Include(x => x.ActivityType)
                    .ThenInclude(x => x.EmissionsSource)
                    .Include(x => x.FuelType)
                    .Where(x => x.isProcessed != true
                    && x.ActivityType.EmissionsSource.Id == emissionSourceId
                    && x.OrganizationUnit.Organization.Id.ToString() == organizationId
                    && x.FuelType.Name == "Natural Gas"
                    && x.isProcessed == false)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"Method: GetStationaryCombustionActivityData - Exception: {ex}");
                return null;
            }

        }
    }
}
