using Abp.AutoMapper;
using ClimateCamp.Common.Authentication.External;

namespace ClimateCamp.Common.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
