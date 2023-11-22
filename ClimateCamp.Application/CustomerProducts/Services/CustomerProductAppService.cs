using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    public class CustomerProductAppService : AsyncCrudAppService<CustomerProduct, CustomerProductDto, Guid, CustomerProductPagedResultDto, CreateCustomerProductDto, CreateCustomerProductDto>, ICustomerProductAppService
    {
        private readonly IRepository<CustomerProduct, Guid> _customerProductRepository;
        private readonly ILogger<CustomerProductAppService> _logger;

        /// <param name="customerProductRepository"></param>
        /// <param name="logger"></param>
        public CustomerProductAppService(IRepository<CustomerProduct, Guid> customerProductRepository,
            ILogger<CustomerProductAppService> logger) : base(customerProductRepository)
        {
            _customerProductRepository = customerProductRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get Customer Product by productId and customer organizationId
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<CustomerProductDto> GetCustomerProduct(Guid productId, Guid organizationId)
        {
            try
            {
                var data = await _customerProductRepository
                           .GetAll()
                           .Where(x => x.ProductId == productId && x.OrganizationId == organizationId)
                           .FirstOrDefaultAsync();

                return ObjectMapper.Map<CustomerProductDto>(data);
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetCustomerProductDto - Exception: {exception}");

                return null;
            }
        }
    }
}