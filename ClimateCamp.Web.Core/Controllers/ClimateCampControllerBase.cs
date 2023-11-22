using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using ClimateCamp.Core;
using Microsoft.AspNetCore.Identity;

namespace ClimateCamp.Controllers
{
    public abstract class ClimateCampControllerBase : AbpController
    {
        protected ClimateCampControllerBase()
        {
            LocalizationSourceName = ClimateCampConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
