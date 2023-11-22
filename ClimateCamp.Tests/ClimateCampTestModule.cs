using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFrameworkCore;
using Castle.MicroKernel.Registration;
using ClimateCamp.Application;
using ClimateCamp.EntityFrameworkCore;
using ClimateCamp.Infrastructure.FileUploadService;
using ClimateCamp.Tests.DependencyInjection;
using NSubstitute;
using System;

namespace ClimateCamp.Tests
{
    [DependsOn(
        typeof(CommonApplicationModule),
        typeof(ClimateCampEntityFrameworkModule),
        typeof(AbpTestBaseModule)
        )]
    public class ClimateCampTestModule : AbpModule
    {
        public ClimateCampTestModule(ClimateCampEntityFrameworkModule entityFrameworkModule)
        {
            entityFrameworkModule.SkipDbContextRegistration = true;
            entityFrameworkModule.RunInitialDataSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator<CommonDbContext>>();
            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager); //

            RegisterFakeService<BlobFileUploadService>();

            RegisterFakeService<EmailClient.Services.EmailClientSender>();

            RegisterFakeService<Infrastructure.HubSpot.IHubspotService>();

            RegisterFakeService<ClimateCamp.Infrastructure.AzureADB2C.GraphClientService>();

            RegisterFakeService<Castle.Core.Logging.ILogger>();

            RegisterFakeService<ClimateCampTestData>();

        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            
        }

        private void RegisterFakeService<TService>() where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }
    }
}
