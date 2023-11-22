using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.MultiTenancy;
using ClimateCamp.Application.Products.Dto;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Common.Users.Dto;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using ClimateCamp.Infrastructure.FileUploadService;
using ClimateCamp.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{

    [AbpAuthorize]
    public class ProductsAppService : AsyncCrudAppService<Product, ProductDto, Guid, PagedProductsResultDto, CreateProductDto, CreateProductDto>, IProductsAppService
    {
        private readonly IRepository<Product, Guid> _productsRepository;
        private readonly IRepository<PurchasedProductsData, Guid> _purchasedProductsDataRepository;
        private readonly IRepository<Emission, Guid> _emissionRepository;
        private readonly ILogger<ProductsAppService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<ProductEmissions, Guid> _productEmissionsRepository;
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<CustomerProduct, Guid> _customerProductRepository;

        public ProductsAppService(IRepository<Product, Guid> productsRepository,
            IRepository<ActivityData, Guid> activityDataRepository,
            IRepository<PurchasedProductsData, Guid> purchasedProductsDataRepository,
            IRepository<Emission, Guid> emissionRepository,
            ILogger<ProductsAppService> logger,
            IFileUploadService fileUploadService,
            IRepository<User, long> userRepository,
            IRepository<Organization, Guid> organizationRepository,
            IRepository<ProductEmissions, Guid> productEmissionsRepository,
            IRepository<CustomerProduct, Guid> customerProductRepository
            ) : base(productsRepository)
        {
            _purchasedProductsDataRepository = purchasedProductsDataRepository;
            _productsRepository = productsRepository;
            _emissionRepository = emissionRepository;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _productEmissionsRepository = productEmissionsRepository;
            _activityDataRepository = activityDataRepository;
            _customerProductRepository = customerProductRepository;
        }

        public async Task<PagedResultDto<ProductDto>> GetAllProductsByOrganizationId(Guid organizationId)
        {

            var products = await _productsRepository.GetAll()
                .Include(x => x.Organization)
                .Where(x => x.Organization.Id == organizationId)
                .ToListAsync();

            var result = new PagedResultDto<ProductDto>()
            {
                Items = ObjectMapper.Map<List<ProductDto>>(products),
                TotalCount = products.Count
            };

            return result;
        }

        /// <summary>
        /// Method to populate the Request Table View.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductRequestManagementDto>> GetAllProductsManagementRequestData(Guid organizationId)
        {
            //look at all purchase activity data, grouped by individual product, keep the latest create user info, grouped by each organization.
            //all activity data with a product in pending state

            try
            {
                var purchases = await _purchasedProductsDataRepository.GetAll()
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductEmissions)
                .Include(x => x.Product.CustomerProducts)
                .Include(x => x.CreatorUser)
                .Include(x => x.OrganizationUnit)
                .Where(x => x.Product.OrganizationId == organizationId)
                .OrderBy(x => x.Product.Status).ThenByDescending(x => x.CreationTime)
                .GroupBy(k => new { k.ProductId, k.OrganizationUnit.OrganizationId })
                .Select(g => new ProductRequestManagementDto
                {
                    Id = g.First().Id,
                    CreatorUser = ObjectMapper.Map<UserDto>(g.First().CreatorUser),
                    Name = g.First().Product.Name,
                    Status = g.First().Product.Status,
                    OrganizationId = g.Key.OrganizationId,
                    Accuracy = g.First().Product.Accuracy,
                    ProductCode = g.First().Product.ProductCode,
                    CO2eq = (g.First().Product.ProductEmissions != null && g.First().Product.ProductEmissions.Any()) ?
                                  g.First().Product.ProductEmissions.Single().CO2eq : null,
                    CO2eqUnitId = (g.First().Product.ProductEmissions != null && g.First().Product.ProductEmissions.Any()) ?
                                  g.First().Product.ProductEmissions.Single().CO2eqUnitId : null,
                    UnitId = g.First().UnitId ?? 0,
                    IsActive = g.First().Product.IsActive,
                    ImagePath = g.First().Product.ImagePath,
                    ProductId = g.First().ProductId,
                    ProductCorrelated = g.First().Product.Status == (int)ProductStatus.Shared && 
                                        g.First().Product.ProductEmissions.Any(x => x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product) &&
                                        g.First().Product.CustomerProducts.Any(x => x.OrganizationId == g.Key.OrganizationId) ? 
                                        g.First().Product.Name : null,
                    ProductRequested = g.First().Product.CustomerProducts.Any() ? GetProductRequested(g.First().Product.CustomerProducts) : g.First().Product.Name,
                    IsUsedStatus = g.First().Product.Status == (int)ProductStatus.Shared &&
                                   g.First().Product.ProductEmissions.Any(x => x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product && x.IsSelected == true ),
                })
                .ToListAsync();


                var result = new PagedResultDto<ProductRequestManagementDto>()
                {
                    Items = purchases,
                    TotalCount = purchases.Count
                };

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method: GetAllProductsManagementRequestData - Exception: {ex}");
                return null;
            }
        }

        private static string GetProductRequested(ICollection<CustomerProduct> customerProducts)
        {
            if(!customerProducts.Any())
            {
                return null;
            }
            else
            {
                return customerProducts.First().Name;
            }
        }

        /// <summary>
        /// Method to populate the Outgoing Requests table with product requests sent to suppliers.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductRequestManagementDto>> GetSupplierProductRequestData(Guid organizationId)
        {
            //look at all purchase activity data, grouped by individual product, keep the latest create user info, grouped by each organization.
            try
            {

                var purchases =  _purchasedProductsDataRepository.GetAll()
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductEmissions)
                .ThenInclude(x => x.Unit)
                .Include(p => p.Product.Organization)
                .Include(x => x.OrganizationUnit)
                .Include(x => x.Product.CustomerProducts)
                .ThenInclude(x => x.Unit)
                .Where(x => x.OrganizationUnit.OrganizationId == organizationId
                    && x.Product.OrganizationId != null //orphan products excluded
                    && x.IsDeleted == false
                    && x.Product.IsDeleted == false)
                 .OrderBy(x => x.Status).ThenByDescending(x => x.CreationTime)
                .GroupBy(k => new { k.ProductId })
                .Select(g => new ProductRequestManagementDto
                {
                    Id = g.First().Id,
                    CreatorUser = ObjectMapper.Map<UserDto>(g.First().CreatorUser), //user from customer's organization who last purchased the product
                    Name = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().Name : g.First().Product.Name,
                    Description = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().Description : g.First().Product.Description,
                    Status = g.First().Product.Status,
                    OrganizationId = g.First().Product.OrganizationId,// supplier organization Id
                    SupplierOrganization = g.First().Product.Organization.Name ?? "Customer",
                    SupplierOrganizationImage = g.First().Product.Organization.PicturePath ?? "assets/img/user-neutral.svg",
                    Accuracy = g.First().Product.Accuracy,
                    ProductCode = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().ProductCode : g.First().Product.ProductCode,
                    CO2eq = (g.First().Product.ProductEmissions != null && g.First().Product.ProductEmissions.Any(x => x.IsSelected == true)) ?
                                   g.First().Product.ProductEmissions.First(x => x.IsSelected == true).CO2eq : null, //g.First().Product.CO2eq,
                    CO2eqUnitId = (g.First().Product.ProductEmissions != null && g.First().Product.ProductEmissions.Any()) ?
                                   g.First().Product.ProductEmissions.First(x => x.IsSelected == true).CO2eqUnitId : null, //g.First().Product.CO2eqUnitId,
                    UnitId = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().UnitId ?? 0 : g.First().UnitId ?? 0,
                    ProductUnit = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().Unit.Name : string.Empty,
                    EmissionUnit = (g.First().Product.ProductEmissions != null && g.First().Product.ProductEmissions.Any()) ?
                                          g.First().Product.ProductEmissions.First(x => x.IsSelected == true).Unit.Name : string.Empty,
                    IsActive = g.First().Product.IsActive,
                    ImagePath = g.First().Product.CustomerProducts.Any() ? g.First().Product.CustomerProducts.First().ImagePath : g.First().Product.ImagePath,
                    ProductId = g.First().ProductId,
                    IsUsedStatus = g.First().Product.Status == (int)ProductStatus.Shared &&
                                   g.First().Product.ProductEmissions.Any(x => x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product && x.IsSelected == true),
                })
                .ToList();


                var result = new PagedResultDto<ProductRequestManagementDto>()
                {
                    Items = purchases,
                    TotalCount = purchases.Count
                };

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method: GetSupplierProductRequestData - Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Retrieves all the Products of an Organization that have the 'shared' Status. 
        /// This will populate the drop down of previously approved products, for updating data or merging duplicate products entries.  
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductRequestManagementDto>> GetAllSharedProductsManagementRequestData(Guid organizationId)
        {
            var products = await _productsRepository.GetAll()
        .Include(x => x.ProductEmissions)
        .Include(x => x.CreatorUser)
        //show only products created by organization itself or those whose requests are already approved
        .Where(x => x.CreatorUser.OrganizationId == organizationId || (x.OrganizationId == organizationId && x.Status == 1))
        .OrderBy(x => x.Name)
        .Select(x => new ProductRequestManagementDto
        {
            Id = x.Id,
            // CreatorUser = ObjectMapper.Map<UserDto>(x.CreatorUser),
            Name = x.Name,
            Status = x.Status,
            OrganizationId = x.OrganizationId,
            Accuracy = x.Accuracy,
            ProductCode = x.ProductCode,
            CO2eq = (x.ProductEmissions != null && x.ProductEmissions.Any()) ? x.ProductEmissions.Single().CO2eq : null, //x.CO2eq,
            CO2eqUnitId = (x.ProductEmissions != null && x.ProductEmissions.Any()) ? x.ProductEmissions.Single().CO2eqUnitId : null, //x.CO2eqUnitId,
            UnitId = x.UnitId ?? 0,
            IsActive = x.IsActive,
            ImagePath = x.ImagePath,
        })
        .ToListAsync();

            var result = new PagedResultDto<ProductRequestManagementDto>()
            {
                Items = products,
                TotalCount = products.Count
            };

            return result;
        }

        /// <summary>
        /// Retrieves all shared products of a verified organization and all products from pending requests sent by the currently logged in organization.
        /// This will populate the dropdown of previously added products 
        /// </summary>
        /// <param name="supplierOrganizationId"></param>
        /// <param name="currentOrganizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductRequestManagementDto>> GetAllVisibleProducts(Guid supplierOrganizationId, Guid currentOrganizationId)
        {
            var products = from p in _productsRepository.GetAll()
                           join pe in _productEmissionsRepository.GetAll().Where(x => x.IsSelected == true) on p.Id equals pe.ProductId into ppe
                           from pe in ppe.DefaultIfEmpty()
                           join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id into pu
                           from u in pu.DefaultIfEmpty()
                           join o in _organizationRepository.GetAll() on p.OrganizationId equals o.Id into po
                           from o in po.DefaultIfEmpty()
                           where p.OrganizationId == supplierOrganizationId &&
                              // Temporarily not showing Shared products of other organizations until product access management is implemented
                              // (p.Status == (int)ProductStatus.Shared ||
                              ((u.OrganizationId == currentOrganizationId &&
                              (p.Status == (int)ProductStatus.Shared || p.Status == (int)ProductStatus.PendingRequest)))
                           orderby p.Name
                           select new ProductRequestManagementDto
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Status = p.Status,
                               OrganizationId = p.OrganizationId,
                               Accuracy = p.Accuracy,
                               ProductCode = p.ProductCode, 
                               CO2eq = pe.CO2eq,//CO2eq = productEmission != null ? productEmission.CO2eq : null,                         
                               CO2eqUnitId = pe.CO2eqUnitId, //CO2eqUnitId = productEmission != null ? productEmission.CO2eqUnitId : null,
                               UnitId = p.UnitId ?? 0,
                               IsActive = p.IsActive,
                               ImagePath = p.ImagePath,

                           };

            var result = await products.ToListAsync();

            return new PagedResultDto<ProductRequestManagementDto>()
            {
                Items = result,
                TotalCount = result.Count
            };

        }

        public async Task<List<ProductRequestManagementDto>> GetAllOrphanProductsByOrganizationId(Guid organizationId)
        {
            var orphanProducts = from p in _productsRepository.GetAll()
                                 join productEmission in _productEmissionsRepository.GetAll()
                                 on p.Id equals productEmission.ProductId into productEmissions
                                 from productEmission in productEmissions.DefaultIfEmpty()
                                 join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id into pu
                                 from u in pu.DefaultIfEmpty()
                                 where p.OrganizationId == null
                                    && u.OrganizationId == organizationId
                                    && p.Status == (int)ProductStatus.Orphan
                                 orderby p.Name
                                 select new ProductRequestManagementDto
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Status = p.Status,
                                     Accuracy = p.Accuracy,
                                     ProductCode = p.ProductCode,
                                     CO2eq = (p.ProductEmissions != null && p.ProductEmissions.Any()) ? p.ProductEmissions.Single().CO2eq : null, //p.CO2eq,
                                     CO2eqUnitId = (p.ProductEmissions != null && p.ProductEmissions.Any()) ? p.ProductEmissions.Single().CO2eqUnitId : null, //p.CO2eqUnitId,
                                     UnitId = p.UnitId ?? 0,
                                     IsActive = p.IsActive,
                                     ImagePath = p.ImagePath,
                                 };

            var result = await orphanProducts.ToListAsync();
            return result;
        }

        public async Task<ProductDto> UpdateProductStatus(CreateProductDto input, Guid selectedProductRequestId, Guid selectedProductOrganizationId)
        {
            try
            {
                var product = _productsRepository.Single(x => x.Id == input.Id);

                if (input.Status == (int)ProductStatus.Rejected)
                {

                    product.Status = input.Status;

                    await _productsRepository.UpdateAsync(product);
                }
                else
                {
                    product.Accuracy = 2;
                    product.Status = input.Status;

                    var productRequest = _purchasedProductsDataRepository.Single(x => x.Id == selectedProductRequestId);

                    // update product purchase with correlating product id

                    var prevProductRequestId = productRequest.ProductId;
                    // replace existing benchmark purchase history and product emission with correlating supplier  product
                    productRequest.ProductId = input.Id;

                    var customerProduct = await _customerProductRepository.FirstOrDefaultAsync(x => x.OrganizationId == selectedProductOrganizationId && x.ProductId == prevProductRequestId);

                    await _productsRepository.DeleteAsync(prevProductRequestId);

                    await _purchasedProductsDataRepository.UpdateAsync(productRequest);

                    await _productsRepository.UpdateAsync(product);

                    if(customerProduct != null)
                    {
                        customerProduct.ProductId = product.Id;

                        await _customerProductRepository.UpdateAsync(customerProduct);
                    }

                    if (input.ProductEmissions != null && input.ProductEmissions.Any())
                    {

                        input.ProductEmissions.First().EmissionSourceType = (int)ProductEmissionTypeEnum.Product;

                        await InsertProductEmissions(prevProductRequestId, input.ProductEmissions.First(), input.ProductEmissions.First().CustomerOrganizationId ?? Guid.Empty);

                        //Update product emissions (Benchmark + Product) with correlating product id

                        var allProductEmissions = await _productEmissionsRepository
                            .GetAll()
                            .Where(x => x.ProductId == prevProductRequestId &&
                            x.CustomerOrganizationId == input.ProductEmissions.First().CustomerOrganizationId)
                            .ToListAsync();

                        allProductEmissions.ForEach(async emission =>
                        {
                            emission.ProductId = product.Id;

                            await _productEmissionsRepository.UpdateAsync(emission);
                        });

                        var orphanProductEmission = await _productEmissionsRepository
                            .GetAll()
                            .Where(x => x.ProductId == product.Id &&
                            x.CustomerOrganizationId == input.ProductEmissions.First().CustomerOrganizationId &&
                            x.EmissionSourceType == null)
                            .FirstOrDefaultAsync();

                        await _productEmissionsRepository.DeleteAsync(orphanProductEmission);
                    }

                    await InsertPurchasedProductEmissions(input, product, (int)ProductEmissionTypeEnum.Product);

                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                return ObjectMapper.Map<ProductDto>(product);

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method: UpdateProductStatus - Exception: {ex}");
                return null;
            }
        }

        public async Task<ProductDto> AddSharedProductwithPurchaseProductData(CreateProductDto input, Guid purchasedProductId)
        {
            try
            {
                var purchasedProduct = _purchasedProductsDataRepository.Get(purchasedProductId);
                var product = _productsRepository.FirstOrDefault(x => x.Id == purchasedProduct.ProductId);

                product.Name = input.Name;
                product.ProductCode = input.ProductCode;
                product.Status = (int)ProductStatus.Shared;
                product.Accuracy = 2;
                product.UnitId = input.UnitId;
                product.OrganizationId = input.OrganizationId;

                await _productsRepository.UpdateAsync(product);

                await CurrentUnitOfWork.SaveChangesAsync();

                if (input.ProductEmissions != null && input.ProductEmissions.Any())
                {
                    input.ProductEmissions.ToList().ForEach(async prdEmission =>
                    {
                        await InsertProductEmissions(product.Id, prdEmission, prdEmission.CustomerOrganizationId ?? Guid.Empty);
                    });
                }

                int emissionSourceType = input.ProductEmissions != null && input.ProductEmissions.Any() ?
                                         input.ProductEmissions.First().EmissionSourceType ?? 0 : 0;

                await InsertPurchasedProductEmissions(input, product, emissionSourceType);

                return ObjectMapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Method: AddSharedProductwithPurchaseProductData - Exception: {ex}");
                return null;
            }
        }


        /// <summary>
        /// Upload Product Logo To Azure Blob Storage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Boolean> UploadProductLogo(IFormFile file, [FromForm] Guid productId)
        {
            if (productId != Guid.Empty)
            {
                var product = await _productsRepository.GetAsync(productId);

                if (product == null || file == null)
                    return false;

                var productLogo = new UploadFileModel()
                {
                    BlobContainerName = SettingManager.GetSettingValue("OrganizationBlobContainerName"),
                    File = file,
                    FileNameWithExtension = file.FileName,
                    Path = Convert.ToString(MultiTenancyConsts.DefaultTenantId) + "/organizations/" + product.OrganizationId + "/products/" + product.Id,
                };
                var url = await _fileUploadService.UploadFileAsync(productLogo);

                if (!string.IsNullOrEmpty(url))
                {
                    product.ImagePath = url;

                    await _productsRepository.UpdateAsync(product);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calculate and insert emissions for Products only if they are missing
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        /// <param name="emissionSourceType"></param>
        /// <returns></returns>
        private async Task<bool> InsertPurchasedProductEmissions(CreateProductDto input, Product product, int emissionSourceType)
        {
            var organizationsPurchaseProducts = await _purchasedProductsDataRepository.GetAll()
                                                    .Include(x => x.OrganizationUnit)
                                                    .ThenInclude(x => x.Organization)
                                                    .Where(x => x.ProductId == input.Id
                                                    && x.OrganizationUnit.OrganizationId == input.OrganizationId).ToListAsync(); //input.OrganizationId

            foreach (var purchasedProduct in organizationsPurchaseProducts)
            {
                var isExist = _emissionRepository.FirstOrDefault(x => x.ActivityDataId == purchasedProduct.Id);
                if (isExist == null)
                {
                    var productEmission = input.ProductEmissions.Where(x => x.EmissionSourceType == emissionSourceType).FirstOrDefault();

                    var emissionActivityModel = new Emission
                    {
                        OrganizationUnitId = purchasedProduct.OrganizationUnitId,
                        EmissionsDataQualityScore = EmissionsDataQualityScore.Estimated,
                        EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                        ResponsibleEntityID = product.Id,
                        ResponsibleEntityType = (int)ResponsibleEntityTypes.Product,
                        IsActive = false,
                        CO2E = (purchasedProduct.Quantity * productEmission?.CO2eq),
                        CO2EUnitId = purchasedProduct.UnitId,
                        CreationTime = DateTime.UtcNow,
                        CreatorUserId = AbpSession.UserId,
                        ActivityDataId = purchasedProduct.Id,

                    };
                    await _emissionRepository.InsertAsync(emissionActivityModel);
                }
            }

            return true;
        }

        public override async Task<ProductDto> GetAsync(EntityDto<Guid> input)
        {
            var product = await _productsRepository
                .GetAll()
                .Include(x => x.ProductEmissions)
                .ThenInclude(x => x.ProductsEmissionSources)
                .Where(x => x.Id == input.Id)
                .SingleAsync();

            return ObjectMapper.Map<ProductDto>(product);
        }

        public override async Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            var product = await base.CreateAsync(input);

            if (input.ProductEmissions.Any())
            {

                input.ProductEmissions.First().Id = Guid.Empty;

                input.ProductEmissions.First().EmissionSourceType = (int)ProductEmissionTypeEnum.Benchmark;

                input.ProductEmissions.First().IsSelected = true;

                await InsertProductEmissions(product.Id, input.ProductEmissions.First(), input.ProductEmissions.First().CustomerOrganizationId ?? Guid.Empty);
            }

            return product;
        }

        private async Task<bool> InsertProductEmissions(Guid productId, ProductEmissionsDto productEmission, Guid customerOrganizationId)
        {
            try
            {
                var productEmissionData = await _productEmissionsRepository
                    .GetAll()
                    .Where(x => x.ProductId == productId &&
                    x.CustomerOrganizationId == customerOrganizationId &&
                    x.EmissionSourceType == (int)ProductEmissionTypeEnum.Benchmark)
                    .FirstOrDefaultAsync();

                if (productEmissionData == null)
                {
                    productEmission.ProductId = productId;
                    productEmission.IsSelected = true;

                    await _productEmissionsRepository.InsertAsync(ObjectMapper.Map<ProductEmissions>(productEmission));
                }

                else
                {
                    productEmissionData.Id = Guid.Empty;
                    productEmissionData.EmissionSourceType = productEmission.EmissionSourceType;
                    productEmissionData.CO2eq = productEmission.CO2eq;
                    productEmissionData.CO2eqUnitId = productEmission.CO2eqUnitId;
                    productEmissionData.IsSelected = false;

                    await _productEmissionsRepository.InsertAsync(productEmissionData);
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: InsertProductEmissions - Exception: {exception}");
                return false;
            }
        }

        /// <summary>
        /// Get all products by organization Id (excluding orphan products as they are not owned by that organization).
        /// Used to populate the 'Products' tab of 'My Products'.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="periodType"></param>
        /// <param name="period"></param>
        /// <param name="year"></param>
        /// <param name="getTreeTableFormData"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductDto>> GetAllParentAndChildProductsByOrganizationId(Guid organizationId, int periodType, int period, int year, bool getTreeTableFormData = false)
        {
            try
            {
                // get all products that belong to the organization (excluding orphan products added by that organization) and their emissions depending on periodType (yearly, quarterly or monthly)
                // products added by another organization are only included if their status is Shared - meaning the current organization acknowledges these products
                // if getTreeTableFormData = true, the products are nested by ParentProductId

                //Get the List of all the products matching these conditions 
                var productsData = await _productsRepository.GetAll()
                 .Include(x => x.Products)
                 .Include(x => x.CustomerProducts)
                 .Include(x => x.ProductEmissions)
                 .ThenInclude(x => x.ProductsEmissionSources)
                 .Include(x => x.CreatorUser)
                 .Where(x => x.OrganizationId == organizationId && x.Status != (int)ProductStatus.Orphan && (x.CreatorUser.OrganizationId == organizationId || x.Status == (int)ProductStatus.Shared))
                 .OrderBy(x => x.Name)
                 .ToListAsync();

                //Get the List of the latest Product emission of each product that belongs to productsData
                var latestProductEmissions = productsData
                .SelectMany(x => x.ProductEmissions
                .Where(x => x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product)
                .GroupBy(x => x.ProductId)
                .Select(g => g.OrderByDescending(x => x.Year).FirstOrDefault()))
                .ToList();

                var dbContext = _productsRepository.GetDbContext();

                foreach (var product in productsData)
                {
                    if (product.CustomerProducts.Any())
                    {
                        product.Name = product.CustomerProducts.First().Name;
                        product.ProductCode = product.CustomerProducts.First().ProductCode;
                        product.UnitId = product.CustomerProducts.First().UnitId;
                    }

                    // Detach existing ProductEmissions entities so the new collection can be assigned without triggering deletions
                    dbContext.Entry(product).Collection(p => p.ProductEmissions).Query().ToList().ForEach(productEmissions => dbContext.Entry(productEmissions).State = EntityState.Detached);
                    //Match each product with the corresponding ProductEmissions
                    product.ProductEmissions = latestProductEmissions.Where(x => x.ProductId == product.Id).ToList();
                }

                var recursiveProductsData = getTreeTableFormData ? productsData.Where(x => x.ParentProductId == null).ToList() : productsData;

                var result = new PagedResultDto<ProductDto>()
                {
                    Items = ObjectMapper.Map<List<ProductDto>>(recursiveProductsData),
                    TotalCount = recursiveProductsData.Count
                };

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetAllParentAndChildProductsByOrganizationId - Exception: {exception}");
                return null;
            }
        }

        public async Task<List<ProductEmissionTypesVM>> GetProductEmissionTypes(Guid productId, Guid organizationId)
        {
            try
            {
                var productEmissionsVM = new List<ProductEmissionTypesVM>();

                #region BenchMark Emission

                var benchMarkEmission = await _productEmissionsRepository
                   .GetAll()
                   .Where(x => x.ProductId == productId && x.CustomerOrganizationId == organizationId &&
                    x.EmissionSourceType == (int)ProductEmissionTypeEnum.Benchmark)
                   .FirstOrDefaultAsync();

                if (benchMarkEmission != null)
                {
                    var purchasedProduct = await _purchasedProductsDataRepository
                        .GetAll()
                        .Include(x => x.OrganizationUnit)
                        .ThenInclude(x => x.Organization)
                        .Where(x => x.ProductId == productId && x.OrganizationUnit.OrganizationId == organizationId)
                        .FirstOrDefaultAsync();

                    var activityData = await _activityDataRepository
                        .GetAll()
                        .Include(x => x.EmissionFactor)
                        .ThenInclude(x => x.Library)
                        .Where(x => x.Id == purchasedProduct.Id)
                        .SingleOrDefaultAsync();

                    productEmissionsVM.Add(new ProductEmissionTypesVM
                    {
                        Id = benchMarkEmission.Id,
                        CO2eq = benchMarkEmission?.CO2eq,
                        CO2eqUnitId = benchMarkEmission?.CO2eqUnitId,
                        EmissionFactorName = activityData?.EmissionFactor?.Library?.Name ?? "N/A",
                        EmissionFactorYear = activityData?.EmissionFactor?.Library?.Year,
                        Type = (int)ProductEmissionTypeEnum.Benchmark,
                        EmissionTypeName = Enum.GetName(typeof(ProductEmissionTypeEnum), ProductEmissionTypeEnum.Benchmark),
                        IsSelected = benchMarkEmission?.IsSelected ?? false
                    });

                }

                #endregion

                #region Product Emission

                var productEmission = await _productEmissionsRepository
                    .GetAll()
                    .Include(x => x.Product)
                    .ThenInclude(x => x.Organization)
                    .Where(x => x.ProductId == productId && x.CustomerOrganizationId == organizationId &&
                    (x.Product != null ? x.Product.Status == (int)ProductStatus.Shared : true) && x.EmissionSourceType == (int)ProductEmissionTypeEnum.Product)
                    .OrderByDescending(x => x.Year)
                    .FirstOrDefaultAsync();

                if (productEmission != null)
                {

                    productEmissionsVM.Add(new ProductEmissionTypesVM
                    {
                        Id = productEmission.Id,
                        CO2eq = productEmission?.CO2eq,
                        CO2eqUnitId = productEmission?.CO2eqUnitId,
                        EmissionFactorName = null,
                        EmissionFactorYear = null,
                        OrganizationName = productEmission?.Product?.Organization?.Name,
                        Type = (int)ProductEmissionTypeEnum.Product,
                        EmissionTypeName = Enum.GetName(typeof(ProductEmissionTypeEnum), ProductEmissionTypeEnum.Product),
                        IsSelected = productEmission?.IsSelected ?? false
                    });

                }

                #endregion

                #region Organization Emission

                var orgEmission = await _productEmissionsRepository
                   .GetAll()
                   .Include(x => x.Product)
                   .ThenInclude(x => x.Organization)
                   .Where(x => x.ProductId == productId && x.CustomerOrganizationId == organizationId &&
                   (x.Product != null ? x.Product.Status == (int)ProductStatus.Shared : true) && x.EmissionSourceType == (int)ProductEmissionTypeEnum.Organization)
                   .OrderByDescending(x => x.Year)
                   .FirstOrDefaultAsync();

                if (orgEmission != null)
                {

                    productEmissionsVM.Add(new ProductEmissionTypesVM
                    {
                        Id = orgEmission.Id,
                        CO2eq = orgEmission?.CO2eq,
                        CO2eqUnitId = orgEmission?.CO2eqUnitId,
                        EmissionFactorName = null,
                        EmissionFactorYear = null,
                        OrganizationName = orgEmission?.Product?.Organization?.Name,
                        Type = (int)ProductEmissionTypeEnum.Organization,
                        EmissionTypeName = Enum.GetName(typeof(ProductEmissionTypeEnum), ProductEmissionTypeEnum.Organization),
                        IsSelected = orgEmission?.IsSelected ?? false
                    });

                }

                #endregion

                return productEmissionsVM;

            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetAllEmissionsForEmissionTypes - Exception: {exception}");
                return null;
            }
        }

        public async Task<bool> ConfirmEmissionFactorSelection(ConfirmEmissionFactorSelectionRequestModel confirmEmissionFactorSelectionRequestModel, int selectedProductUnitAttribute)
        {
            try
            {
                // get all the product emisisons from ProductEmissions table for that product and customer organization Id
                var productEmissions = await _productEmissionsRepository
                    .GetAll()
                    .Where(x => x.ProductId ==
                    confirmEmissionFactorSelectionRequestModel.ProductId &&
                    x.CustomerOrganizationId ==
                    confirmEmissionFactorSelectionRequestModel.OrganizationId &&
                    x.PeriodType == 3)
                    .ToListAsync();

                //set the respective emission type is selected to true
                foreach (var productEmission in productEmissions)
                {
                    if (productEmission.Id == confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.Id)
                        productEmission.IsSelected = true;
                    else
                        productEmission.IsSelected = false;

                    await _productEmissionsRepository.UpdateAsync(productEmission);
                }

                foreach (var period in confirmEmissionFactorSelectionRequestModel.SelectedPeriods)
                {

                    var productPurchasedData = await (from purchasedProduct in _purchasedProductsDataRepository.GetAll()
                                                       .Include(x => x.OrganizationUnit)
                                                       .ThenInclude(x => x.Organization)
                                                      join activityData in _activityDataRepository.GetAll()
                                                      on purchasedProduct.Id equals activityData.Id
                                                      join emission in _emissionRepository.GetAll()
                                                      on activityData.Id equals emission.ActivityDataId
                                                      where
                                                      purchasedProduct.ProductId ==
                                                      confirmEmissionFactorSelectionRequestModel.ProductId &&
                                                      purchasedProduct.OrganizationUnit.OrganizationId ==
                                                      confirmEmissionFactorSelectionRequestModel.OrganizationId &&
                                                      activityData.ConsumptionStart.Year == period
                                                      select
                                                      new
                                                      {
                                                          PurchasedProduct = purchasedProduct,
                                                          ActivitData = activityData,
                                                          Emission = emission
                                                      })
                                                      .OrderBy(o => o.Emission.Version)
                                                      .LastOrDefaultAsync(); //as purcahed product and activity Data will be single  but emissions could be multiple so in that case need to get the latest version emission


                    if (productPurchasedData != null)
                    {

                        var newEmission = new Emission();

                        newEmission = productPurchasedData.Emission;

                        newEmission.Id = Guid.Empty;
                        newEmission.CO2eFactor = confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.CO2eq;
                        newEmission.CO2eFactorUnitId = confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.CO2eqUnitId;
                        newEmission.CO2E = confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.CO2eq * productPurchasedData.ActivitData.Quantity;
                        newEmission.CO2EUnitId = confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.Type == (int)ProductEmissionTypeEnum.Product ? selectedProductUnitAttribute : confirmEmissionFactorSelectionRequestModel.SelectedProductEmissionType.CO2eqUnitId;
                        newEmission.Version = productPurchasedData.Emission.Version + 1;

                        await _emissionRepository.InsertAsync(newEmission);

                        if(selectedProductUnitAttribute > 0)
                        {
                            productPurchasedData.ActivitData.UnitId = selectedProductUnitAttribute;

                            await _activityDataRepository.UpdateAsync(productPurchasedData.ActivitData);
                        }

                    }

                }

                await CurrentUnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: ConfirmProductEmission - Exception: {exception}");
                return false;
            }
        }

        private int ClosestTo(IEnumerable<int> selectedYears, int year)
        {
            var closestNextYear = year + 1;

            var closestPrevYear = year - 1;

            if (selectedYears.Contains(closestNextYear))
                return closestNextYear;
            else if (selectedYears.Contains(closestPrevYear))
                return closestPrevYear;
            else
                return year;
        }

        public async Task<List<ProductEmissionGroupedVM>> GetAllProductEmissionDetails(Guid productId, Guid organizationId)
        {
            try
            {

                var productEmissions = await (from productEmission in _productEmissionsRepository.GetAll()
                                              .Include(x => x.Product)
                                              .ThenInclude(x => x.Organization)
                                              where
                                              productEmission.ProductId == productId && productEmission.CustomerOrganizationId == organizationId
                                              select productEmission)
                                              .OrderByDescending(x => x.Year)
                                        .ThenBy(x => x.EmissionSourceType)
                                        .ToListAsync();

                var productEmissionsVM = new List<ProductEmissionTypesVM>();

                productEmissionsVM.AddRange(productEmissions.Select(x => new ProductEmissionTypesVM
                {

                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name,
                    OrganizationName = x.Product?.Organization?.Name,
                    CO2eq = x.CO2eq,
                    CO2eqUnitId = x.CO2eqUnitId,
                    Year = x.Year,
                    EmissionTypeName = Enum.GetName(typeof(ProductEmissionTypeEnum), x.EmissionSourceType ?? 0),
                    EmissionType = x.EmissionSourceType
                }));

                foreach (var emission in productEmissionsVM)
                {
                    if (emission.EmissionType == (int)ProductEmissionTypeEnum.Benchmark)
                    {
                        var purchasedProduct = await _purchasedProductsDataRepository
                       .GetAll()
                       .Include(x => x.OrganizationUnit)
                       .ThenInclude(x => x.Organization)
                       .Where(x => x.ProductId == productId && x.OrganizationUnit.OrganizationId == organizationId)
                       .FirstOrDefaultAsync();

                        var activityData = await _activityDataRepository
                            .GetAll()
                            .Include(x => x.EmissionFactor)
                            .ThenInclude(x => x.Library)
                            .Where(x => x.Id == purchasedProduct.Id)
                            .SingleOrDefaultAsync();

                        emission.EmissionFactorName = activityData?.EmissionFactor?.Library?.Name;
                        emission.EmissionFactorYear = activityData?.EmissionFactor?.Library?.Year;
                    }
                }

                //Grouping based on year

                var groupedEmissionsVM = new List<ProductEmissionGroupedVM>();

                groupedEmissionsVM.AddRange(productEmissions.GroupBy(x => x.Year).Select(x => new ProductEmissionGroupedVM
                {
                    Year = x.Key,
                    Emissions = productEmissionsVM.Where(y => y.Year == x.Key).ToList(),
                }));

                return groupedEmissionsVM;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetAllProductEmissionDetails - Exception: {exception}");
                throw;
            }
        }

        public async Task<List<ProductDataConfirmationVM>> GetProductReflectedDataConfirmation(Guid selectedProductId, Guid selectedProductOrganizationId)
        {
            try
            {
                var productDataConfirmationVM = new List<ProductDataConfirmationVM>();

                var customerProduct = await _customerProductRepository.FirstOrDefaultAsync(x => x.OrganizationId == selectedProductOrganizationId && x.ProductId == selectedProductId);

                var supplierProduct = await _productsRepository.FirstOrDefaultAsync(x => x.OrganizationId == selectedProductOrganizationId && x.Id == selectedProductId);

                if(customerProduct != null) // this will be in most of the cases null because of no data in the newly addded customer product table
                {
                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Name", AttributeValue = customerProduct.Name, ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Unit", AttributeValue = Convert.ToString(customerProduct.UnitId), ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Description", AttributeValue = customerProduct.Description ?? "N/A", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Image", AttributeValue = customerProduct.ImagePath ?? "assets/img/user-neutral.svg", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "SKU", AttributeValue = customerProduct.ProductCode ?? "N/A", ProductType = (int)ProductTypeEnum.Customer });
                }
                else
                {
                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Name", AttributeValue = "N/A", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Unit", AttributeValue = "N/A", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Description", AttributeValue = "N/A", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Image", AttributeValue = "assets/img/user-neutral.svg", ProductType = (int)ProductTypeEnum.Customer });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "SKU", AttributeValue =  "N/A", ProductType = (int)ProductTypeEnum.Customer });
                }

                if(supplierProduct != null) // cannot be null as this data is from the products table but added for safety check
                {
                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Name", AttributeValue = supplierProduct.Name, ProductType = (int)ProductTypeEnum.Supplier });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Unit", AttributeValue = Convert.ToString(supplierProduct.UnitId), ProductType = (int)ProductTypeEnum.Supplier });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Description", AttributeValue = supplierProduct.Description ?? "N/A", ProductType = (int)ProductTypeEnum.Supplier });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "Image", AttributeValue = supplierProduct.ImagePath ?? "assets/img/user-neutral.svg", ProductType = (int)ProductTypeEnum.Supplier });

                    productDataConfirmationVM.Add(new ProductDataConfirmationVM { AttributeName = "SKU", AttributeValue = supplierProduct.ProductCode ?? "N/A", ProductType = (int)ProductTypeEnum.Supplier });
                }

                return productDataConfirmationVM;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: GetProductReflectedDataConfirmation - Exception: {exception}");
                return null;
            }
        }

        public async Task<bool> UpdateCustomerProductAttributes(List<ProductDataConfirmationVM> productDataConfirmations, Guid productId, Guid organizationId)
        {
            try
            {
                var customerProduct = await _customerProductRepository.FirstOrDefaultAsync(x => x.OrganizationId == organizationId && x.ProductId == productId);

                if(customerProduct != null)
                {
                    customerProduct.Name = productDataConfirmations.Single(x => x.AttributeName == "Name").AttributeValue;
                    customerProduct.UnitId = productDataConfirmations.Single(x => x.AttributeName == "Unit").AttributeValue != "N/A" ? int.Parse(productDataConfirmations.Single(x => x.AttributeName == "Unit").AttributeValue) : null;
                    customerProduct.Description = productDataConfirmations.Single(x => x.AttributeName == "Description").AttributeValue != "N/A" ? productDataConfirmations.Single(x => x.AttributeName == "Description").AttributeValue : null;
                    customerProduct.ImagePath = productDataConfirmations.Single(x => x.AttributeName == "Image").AttributeValue != "N/A" ? productDataConfirmations.Single(x => x.AttributeName == "Image").AttributeValue : null;
                    customerProduct.ProductCode = productDataConfirmations.Single(x => x.AttributeName == "SKU").AttributeValue != "N/A" ? productDataConfirmations.Single(x => x.AttributeName == "SKU").AttributeValue : null;

                    await _customerProductRepository.UpdateAsync(customerProduct);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: UpdateCustomerProductAttributes - Exception: {exception}");
                return false;
            }
        }

        /// <summary>
        /// Link an orphan product to a supplier by adding the supplier id to the product.
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateProductSupplier(Guid ProductId, Guid SupplierId)
        {
            try
            {

                var product = await _productsRepository.FirstOrDefaultAsync(x => x.Id == ProductId);


                if (product != null && product.OrganizationId == null && product.OrganizationId != SupplierId)
                {
                    product.OrganizationId = SupplierId;
                    await _productsRepository.UpdateAsync(product);
                    return ObjectMapper.Map<ProductDto>(product);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Method: UpdateProductSupplier - Exception: {exception}");
                return null;
            }
        }


    }
}
