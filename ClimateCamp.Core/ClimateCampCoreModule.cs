using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.MultiTenancy;
using ClimateCamp.Common.Timing;
using ClimateCamp.Configuration;
using ClimateCamp.Core.Localization;
using ClimateCamp.Core.Notifications;

namespace ClimateCamp.Core
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class ClimateCampCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            CommonLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = ClimateCampConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Notifications.Providers.Add<ClimateCampAppNotificationProvider>();


            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = ClimateCampConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = ClimateCampConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ClimateCampCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
