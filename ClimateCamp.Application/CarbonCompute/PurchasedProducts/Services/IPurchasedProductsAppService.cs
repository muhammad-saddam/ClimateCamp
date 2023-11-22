using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ClimateCamp.CarbonCompute;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Purchase Products API end points
    /// </summary>
    public interface IPurchasedProductsAppService : IApplicationService
    {
        Task<PurchasedProductsData> AddPurchaseProductAsync(ActivityDataVM input);

        Task<PagedResultDto<PurchasedProductVM>> GetPurchaseProductsByOrganizationId(Guid organizationId, DateTime consumptionStartDate, DateTime consumptionEndDate);

        Task<PurchasedProductsData> UpdatePurchaseProductAsync(ActivityDataVM input, Guid organizationId);
    }
}
