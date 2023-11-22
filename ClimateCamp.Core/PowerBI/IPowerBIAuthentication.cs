using Abp.Application.Services;

namespace ClimateCamp.PowerBI
{
    public interface IPowerBIAuthentication : IApplicationService
    {
        dynamic GetTokenAsync();
    }
}
