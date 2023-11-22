using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application.ProductsEmissionSources.Services
{
    public interface IProductsEmissionSourcesAppService
    {
        Task<bool> SaveProductEmissionSources(List<CreateProductsEmissionSourcesDto> productEmissionSources);
        Task<PagedResultDto<ProductsEmissionSourcesDto>> GetAllProductEmissionSourcesByEmissionId(Guid productEmissionId);
    }
}
