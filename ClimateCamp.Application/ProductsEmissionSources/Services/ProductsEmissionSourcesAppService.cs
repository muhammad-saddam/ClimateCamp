using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application.ProductsEmissionSources.Services
{
    [AbpAuthorize]
    public class ProductsEmissionSourcesAppService : AsyncCrudAppService<ClimateCamp.CarbonCompute.ProductsEmissionSources, ProductsEmissionSourcesDto, Guid, ProductsEmissionSourcesPagedResultDto, CreateProductsEmissionSourcesDto, CreateProductsEmissionSourcesDto>, IProductsEmissionSourcesAppService
    {
        private readonly IRepository<ClimateCamp.CarbonCompute.ProductsEmissionSources, Guid> _productsEmissionSourcesRepository;
        private readonly ILogger<ProductsEmissionSourcesAppService> _logger;

        /// <param name="productsEmissionSourcesRepository"></param>
        /// <param name="logger"></param>
        public ProductsEmissionSourcesAppService
            (
            IRepository<ClimateCamp.CarbonCompute.ProductsEmissionSources, Guid> productsEmissionSourcesRepository,
            ILogger<ProductsEmissionSourcesAppService> logger
            ) : base(productsEmissionSourcesRepository)
        {
            _productsEmissionSourcesRepository = productsEmissionSourcesRepository;
            _logger = logger;
        }

        public async Task<bool> SaveProductEmissionSources(List<CreateProductsEmissionSourcesDto> productEmissionSources)
        {
            try
            {
                foreach (var productEmissionSource in productEmissionSources)
                {
                    if (productEmissionSource.Id == Guid.Empty)
                        await base.CreateAsync(productEmissionSource);
                    else
                        await base.UpdateAsync(productEmissionSource);
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: SaveProductEmissionSources - Exception: {exception}");
                return false;
            }
        }

        public async Task<PagedResultDto<ProductsEmissionSourcesDto>> GetAllProductEmissionSourcesByEmissionId(Guid productEmissionId)
        {
            try
            {
                var productEmissionSources = await _productsEmissionSourcesRepository.GetAll().Where(x => x.ProductEmissionsId == productEmissionId).ToListAsync();

                var result = new PagedResultDto<ProductsEmissionSourcesDto>()
                {
                    Items = ObjectMapper.Map<List<ProductsEmissionSourcesDto>>(productEmissionSources),
                    TotalCount = productEmissionSources.Count
                };

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetAllProductEmissionSources - Exception: {exception}");
                return null;
            }
        }
    }
}
