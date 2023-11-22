using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using ClimateCamp.Configuration;
using ClimateCamp.Core;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.Migrator.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ClimateCamp.Migrator
{
    [DependsOn(typeof(ClimateCampEntityFrameworkModule))]
    public class ClimateCampMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ClimateCampMigratorModule(ClimateCampEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.RunInitialDataSeed = false;

            _appConfiguration = AppConfigurations.Get(
                typeof(ClimateCampMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ClimateCampConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus),
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ClimateCampMigratorModule).GetAssembly());
            
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
