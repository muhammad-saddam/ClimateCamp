using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ClimateCamp.Application.Products.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<ProductDto>> GetAllProductsByOrganizationId(Guid organizationId);

        Task<PagedResultDto<ProductRequestManagementDto>> GetAllProductsManagementRequestData(Guid organizationId);

        Task<PagedResultDto<ProductRequestManagementDto>> GetAllSharedProductsManagementRequestData(Guid organizationId);
        Task<PagedResultDto<ProductRequestManagementDto>> GetAllVisibleProducts(Guid supplierOrganizationId, Guid currentOrganizationId);
        Task<Boolean> UploadProductLogo(IFormFile file, [FromForm] Guid productId);
        Task<List<ProductRequestManagementDto>> GetAllOrphanProductsByOrganizationId(Guid organizationId);
        Task<PagedResultDto<ProductDto>> GetAllParentAndChildProductsByOrganizationId(Guid organizationId, int periodType, int period, int year, bool getTreeTableFormData = false);
        Task<List<ProductEmissionTypesVM>> GetProductEmissionTypes(Guid productId, Guid organizationId);
        Task<bool> ConfirmEmissionFactorSelection(ConfirmEmissionFactorSelectionRequestModel confirmEmissionFactorSelectionRequestModel, int selectedProductUnitAttribute);

        Task<List<ProductEmissionGroupedVM>> GetAllProductEmissionDetails(Guid productId, Guid organizationId);
        Task<ProductDto> UpdateProductSupplier(Guid ProductId, Guid SupplierId);
    }
}
