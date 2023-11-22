using Abp.Authorization;
using ClimateCamp.Common.Authorization.Roles;
using ClimateCamp.Common.Authorization.Users;

namespace ClimateCamp.Core.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
