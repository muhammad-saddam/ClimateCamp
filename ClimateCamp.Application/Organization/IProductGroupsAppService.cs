using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IProductGroupsAppService
    {
        Task<PagedResultDto<ProductGroupsDto>> GetAllProductGroupsData(Guid organizationId,bool getTreeTableFormData = false);
    }
}
