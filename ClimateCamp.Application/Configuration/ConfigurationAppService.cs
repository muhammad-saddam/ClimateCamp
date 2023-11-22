using Abp.Authorization;
using Abp.Runtime.Session;
using ClimateCamp.Application;
using ClimateCamp.Configuration.Dto;
using System.Threading.Tasks;

namespace ClimateCamp.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : CommonAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
