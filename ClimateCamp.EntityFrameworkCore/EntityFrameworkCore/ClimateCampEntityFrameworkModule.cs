using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using ClimateCamp.Core;
using ClimateCamp.EntityFrameworkCore.Seed;

namespace ClimateCamp.EntityFrameworkCore
{
    [DependsOn(
        typeof(ClimateCampCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class ClimateCampEntityFrameworkModule : AbpModule
    {
        /// <summary>
        /// Used in automated tests to skip DBContext registration, in order to use in-memory database of EF Core
        /// </summary>
        public bool SkipDbContextRegistration { get; set; }

        public bool RunInitialDataSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<CommonDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        CommonDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        CommonDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ClimateCampEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (RunInitialDataSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
