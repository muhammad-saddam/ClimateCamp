using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface ICustomerProductAppService
    {
        Task<CustomerProductDto> GetCustomerProduct(Guid productId, Guid organizationId);

    }
}