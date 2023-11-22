using ClimateCamp.Configuration.Dto;
using System.Threading.Tasks;

namespace ClimateCamp.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
