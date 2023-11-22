using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public class EmissionsFactorsDataService : IEmissionsFactorsDataService
    {
        private readonly CommonDbContext _dbContext;
        private readonly ILogger _logger;
        public EmissionsFactorsDataService(CommonDbContext dbContext, ILoggerFactory logger)
        {
            _dbContext = dbContext;
            _logger = logger.CreateLogger<EmissionsFactorsDataService>();
        }
        public async Task<EmissionsFactor> GetEmisionFactors(int emissionsSourceId)
        {
            return await _dbContext.EmissionsFactors.Include(x => x.EmissionsSource)
                .Include(x => x.CO2Unit)
                .Include(x => x.CH4Unit)
                .Include(x => x.N20Unit)
                .Include(x => x.CO2EUnit)
                .FirstOrDefaultAsync(t => t.EmissionsSource.Id == emissionsSourceId);
        }

        public async Task<EmissionsFactor> GetEmissionFactorsByEmissionSourceUnitId(int emissionsSourceId, int unitId, string libraryId)
        {
            EmissionsFactor emissionFactor = new EmissionsFactor();
            try
            {
                emissionFactor = await _dbContext.EmissionsFactors
                .Include(x => x.EmissionsSource)
                .Include(x => x.Unit)
                .Include(x => x.CO2Unit)
                .Include(x => x.CH4Unit)
                .Include(x => x.N20Unit)
                .Include(x => x.CO2EUnit)
                .Include(x => x.Library)
                .FirstOrDefaultAsync(t => t.EmissionsSource.Id == emissionsSourceId
                && t.Unit.Id == unitId
                && t.Library.Id.ToString() == libraryId);
                if (emissionFactor == null)
                {
                    emissionFactor = await _dbContext.EmissionsFactors
                     .Include(x => x.EmissionsSource)
                     .Include(x => x.Unit)
                     .Include(x => x.CO2Unit)
                     .Include(x => x.CH4Unit)
                     .Include(x => x.N20Unit)
                     .Include(x => x.CO2EUnit)
                     .Include(x => x.Library)
                     .FirstOrDefaultAsync(t => t.EmissionsSource.Id == emissionsSourceId
                     && t.Unit.Id == unitId
                     && t.Library.Id.ToString() == _dbContext.EmissionsFactorsLibrary.FirstOrDefault().Id.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method: GetEmisionFactorsByEmissionSouceUnitId - Exception: {ex}");
            }


            return emissionFactor;
        }
        public async Task<Guid?> GetEmissionsFactorsLibraryId(Guid organizationUnitId)
        {
            return await _dbContext.OrganizationUnits.Where(x => x.Id == organizationUnitId).Select(x => x.Organization.EmissionsFactorsLibraryId).FirstOrDefaultAsync();
        }
    }
}