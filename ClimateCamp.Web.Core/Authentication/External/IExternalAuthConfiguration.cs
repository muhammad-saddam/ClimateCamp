using System.Collections.Generic;

namespace ClimateCamp.Common.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
