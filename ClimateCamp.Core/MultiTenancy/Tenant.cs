using Abp.MultiTenancy;
using ClimateCamp.Common.Authorization.Users;

namespace ClimateCamp.Common.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
