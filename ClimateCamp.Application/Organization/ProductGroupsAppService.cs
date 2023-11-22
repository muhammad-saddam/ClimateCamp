using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class ProductGroupsAppService : AsyncCrudAppService<Core.ProductGroups, ProductGroupsDto, Guid, ProductGroupsDto, CreateProductGroupDto, CreateProductGroupDto>, IProductGroupsAppService
    {
        private readonly IRepository<Core.ProductGroups, Guid> _productGroupsRepository;
        
        /// <param name="productGroupsRepository"></param>
        public ProductGroupsAppService(IRepository<Core.ProductGroups, Guid> productGroupsRepository) : base(productGroupsRepository)
        {
            _productGroupsRepository = productGroupsRepository;
        }
        public async Task<PagedResultDto<ProductGroupsDto>> GetAllProductGroupsData(Guid organizationId, bool getTreeTableFormData = false)
        {
            var productsGroupedData = await _productGroupsRepository.GetAll()
                                            .Include(x => x.Children)
                                           .Where(x => x.OrganizationId == organizationId)
                                           .OrderBy(x => x.Name)
                                           .ToListAsync();

            var recursiveProductsGroupedData = getTreeTableFormData ?  productsGroupedData.Where(x => x.ParentProductGroupId == null).ToList() : productsGroupedData;

            var result = new PagedResultDto<ProductGroupsDto>()
            {
                Items = ObjectMapper.Map<List<ProductGroupsDto>>(recursiveProductsGroupedData),
                TotalCount = recursiveProductsGroupedData.Count
            };

            return result;
        }
    }
}
