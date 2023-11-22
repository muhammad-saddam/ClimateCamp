using Abp.Application.Services;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IOrganizationAppService : IApplicationService
    {
        Task<Guid> CreateOrganization(CreateOrganizationDto input);
    }
}
