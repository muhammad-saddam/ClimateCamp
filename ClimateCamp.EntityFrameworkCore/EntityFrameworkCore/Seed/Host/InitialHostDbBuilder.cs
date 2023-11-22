using ClimateCamp.EntityFrameworkCore.Seed.Host.Lookup;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly CommonDbContext _context;

        public InitialHostDbBuilder(CommonDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            #region lookups
            new DefaultCountryCreator(_context).Create();
            #endregion
            new DefaultEditionCreator(_context).Create();
            new DefaultFeatureCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultReductionCreator(_context).Create();
            new DefaultOffsetCreator(_context).Create();
            // new default organization, organization unit creation user story
            #region carbon compute 
            new DefaultVehicleTypeCreator(_context).Create();
            new DefaultUnitCreator(_context).Create();
            new DefaultGreenhouseGasesCreator(_context).Create();
            new DefaultEmissionsSourceCreator(_context).Create();
            new DefaultEmissionsFactorsLibraryCreator(_context).Create();
            new DefaultEmissionsFactorsCreator(_context).Create();
            new DefaultActivityTypeCreator(_context).Create();
            #endregion

            _context.SaveChanges();
        }
    }
}
