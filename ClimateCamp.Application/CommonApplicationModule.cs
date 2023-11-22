using Abp.AutoMapper;
using Abp.FluentValidation;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.Authorization;
using Microsoft.Extensions.Options;

namespace ClimateCamp.Application
{
    [DependsOn(
        typeof(ClimateCampCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule))]
    public class CommonApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<CommonAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<CreateProductDto, Product>()
                      .ForMember(u => u.ProductEmissions, options => options.Ignore());

                config.CreateMap<ProductDto, Product>()
                      .ForMember(u => u.ProductEmissions, options => options.Ignore());

                config.CreateMap<ProductEmissionsDto, ProductEmissions>()
                      .ForMember(u => u.ProductsEmissionSources, options => options.Condition(src => src.ProductsEmissionSources != null));
            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(CommonApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
