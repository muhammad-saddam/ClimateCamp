using Abp.Modules;
using Abp.Reflection.Extensions;
using ClimateCamp.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ClimateCamp.Common.Web.Host.Startup
{
    [DependsOn(
       typeof(CommonWebCoreModule))]
    public class CommonWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public CommonWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CommonWebHostModule).GetAssembly());
        }
    }
}
