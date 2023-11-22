using ClimateCamp.CarbonCompute;
using ClimateCamp.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public class EmissionsDataService : IEmissionDataService
    {

        private readonly CommonDbContext _dbContext;
        public EmissionsDataService(CommonDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ActivityData> SaveActivityData(ActivityData activity)
        {
            await _dbContext.ActivityData.AddAsync(activity);
            await _dbContext.SaveChangesAsync();
            return activity;
        }
        public async Task<int> SaveEmissions(Emission emission)
        {
            try
            {
                var organizationUnit = _dbContext.OrganizationUnits.FirstOrDefault(x => x.Id == emission.OrganizationUnitId);
                if (organizationUnit != null)
                {
                    var dataCollection = _dbContext.DataCollections.Where(x => x.OrganizationId == organizationUnit.OrganizationId).FirstOrDefault();
                    if (dataCollection != null)
                    {
                        dataCollection.LastUpdated = DateTime.UtcNow;
                        _dbContext.DataCollections.Update(dataCollection);
                    }
                }
                await _dbContext.Emissions.AddAsync(emission);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
