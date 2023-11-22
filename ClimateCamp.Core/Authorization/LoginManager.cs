using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.Zero.Configuration;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ClimateCamp.Core.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            INotificationSubscriptionManager notificationSubscriptionManager,


            RoleManager roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        public override Task<AbpLoginResult<Tenant, User>> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            //ToDo, subscribe users on login if not yet subscribed
            return base.LoginAsync(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
        }

        public async Task Subscribe_FileUpdated(int? tenantId, long userId)
        {
            //  await _notificationSubscriptionManager.SubscribeAsync(new UserIdentifier(tenantId, userId), FileUploaded);
        }


    }
}
