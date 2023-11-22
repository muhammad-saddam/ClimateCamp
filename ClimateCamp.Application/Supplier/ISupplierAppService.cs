using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ClimateCamp.Supplier.Dto;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISupplierAppService : IApplicationService
    {
        //Task<PagedResultDto<GetSupplierOutput>> GetMyOrganizationSuppliersAsync();

        //Task InviteSupplierAsync(InviteSupplierInput input);

        Task<PagedResultDto<CustomerSuppliersDto>> GetSuppliersByorganizationId(SuppliersRequestDto input);

        Task<PagedResultDto<CustomerSuppliersDto>> GetSuppliersByCustomerOrganizationIdAndStatusValidated(Guid customrOrganizationId);
    }
}
