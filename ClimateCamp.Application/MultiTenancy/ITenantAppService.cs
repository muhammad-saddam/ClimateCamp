using Abp.Application.Services;
using ClimateCamp.Common.MultiTenancy.Dto;

namespace ClimateCamp.Common.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

