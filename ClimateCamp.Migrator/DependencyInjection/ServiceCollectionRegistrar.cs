using Abp.Dependency;
using Castle.Windsor.MsDependencyInjection;
using ClimateCamp.Core.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClimateCamp.Migrator.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
        }
    }
}
