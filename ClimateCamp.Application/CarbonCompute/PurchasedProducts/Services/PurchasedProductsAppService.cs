using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ClimateCamp.CarbonCompute.GHG;
using ClimateCamp.Application.Common;
using Abp.Authorization;

namespace ClimateCamp.Application
{
    [AbpAuthorize]
    /// <summary>
    /// Purchase Products API end points
    /// </summary>
    public class PurchasedProductsAppService : AsyncCrudAppService<PurchasedProductsData, PurchasedProductsDataDto, Guid, PurchaseProductsResponseDto, CreatePurchaseProductDto, CreatePurchaseProductDto>, IPurchasedProductsAppService
    {
        private readonly IRepository<PurchasedProductsData, Guid> _purchaseProductsRepository;
        private readonly IRepository<ActivityData, Guid> _activityDataRepository;
        private readonly IRepository<ActivityType, int> _activityTypeRepository;
        private readonly IRepository<Emission, Guid> _emissionsRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Unit, int> _unitRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<OrganizationUnit, Guid> _organizationUnitRepository;
        private readonly ILogger<PurchasedProductsAppService> _logger;
        private readonly IRepository<ProductEmissions, Guid> _productEmissionsRepository;
        private readonly IRepository<CustomerProduct, Guid> _customerProductRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseProductsRepository"></param>
        /// <param name="activityDataRepository"></param>
        /// <param name="activityTypeRepository"></param>
        /// <param name="emissionsRepository"></param>
        /// <param name="productRepository"></param>
        /// <param name="unitRepository"></param>
        /// <param name="organizationRepository"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="productEmissionsRepository"></param>
        /// <param name="customerProductRepository"></param>
        /// <param name="logger"></param>
        public PurchasedProductsAppService
            (IRepository<PurchasedProductsData, Guid> purchaseProductsRepository,
             IRepository<ActivityData, Guid> activityDataRepository,
             IRepository<ActivityType, int> activityTypeRepository,
             IRepository<Emission, Guid> emissionsRepository,
             ILogger<PurchasedProductsAppService> logger,
             IRepository<Product, Guid> productRepository,
             IRepository<Unit, int> unitRepository,
             IRepository<Organization, Guid> organizationRepository,
             IRepository<OrganizationUnit, Guid> organizationUnitRepository,
             IRepository<ProductEmissions, Guid> productEmissionsRepository,
             IRepository<CustomerProduct, Guid> customerProductRepository
             ) : base(purchaseProductsRepository)
        {
            _purchaseProductsRepository = purchaseProductsRepository;
            _activityDataRepository = activityDataRepository;
            _activityTypeRepository = activityTypeRepository;
            _emissionsRepository = emissionsRepository;
            _productRepository = productRepository;
            _unitRepository = unitRepository;
            _organizationRepository = organizationRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _logger = logger;
            _productEmissionsRepository= productEmissionsRepository;
            _customerProductRepository = customerProductRepository;
        }

        /// <summary>
        /// fetch list of purchase goods by organizationId
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PurchasedProductVM>> GetPurchaseProductsByOrganizationId(Guid organizationId, DateTime consumptionStartDate, DateTime consumptionEndDate)
        {
            try
            {
                var purchasedGoods = (from prData in _purchaseProductsRepository.GetAll()
                                      join emissionData in _emissionsRepository.GetAll()
                                      on prData.Id equals emissionData.ActivityDataId
                                      into prDataEmissionsGroup
                                      from emissionData in prDataEmissionsGroup.DefaultIfEmpty()
                                      join product in _productRepository.GetAll()
                                      on prData.ProductId equals product.Id
                                      join productEmission in _productEmissionsRepository.GetAll()
                                      on product.Id equals productEmission.ProductId into productEmissions
                                      from productEmission in productEmissions.DefaultIfEmpty()
                                      join supplier in _organizationRepository.GetAll()
                                      on product.OrganizationId equals supplier.Id
                                      join unit in _unitRepository.GetAll()
                                      on prData.UnitId equals unit.Id
                                      join orgUnit in _organizationUnitRepository.GetAll()
                                      on prData.OrganizationUnitId equals orgUnit.Id
                                      join organization in _organizationRepository.GetAll()
                                      on orgUnit.OrganizationId equals organization.Id
                                      where prData.OrganizationUnit.OrganizationId == organizationId && !prData.IsDeleted
                                      && (prData.ConsumptionEnd.Date >= consumptionStartDate.Date && prData.ConsumptionStart.Date <= consumptionEndDate.Date)

                                      select new
                                      {
                                          prData,
                                          emData = (emissionData != null ? emissionData : null),
                                          product,
                                          supplier,
                                          unit,
                                          orgUnit,
                                          organization,
                                          productEmission
                                      })
                                      .AsEnumerable()
                                      .Select(purchasedGood => new PurchasedProductVM
                                      {
                                          Id = purchasedGood.prData.Id,
                                          ProductId = purchasedGood.product.Id,
                                          Name = purchasedGood.prData.Name,
                                          Accuracy = purchasedGood.product.Accuracy,
                                          Status = purchasedGood.product.Status,
                                          StatusName = Enum.GetName(typeof(ProductStatus), purchasedGood.product.Status),
                                          OrganizationUnitId = purchasedGood.orgUnit.Id,
                                          OrganizationId = purchasedGood.product.OrganizationId,
                                          Supplier = purchasedGood.supplier.Name,
                                          OrgUnitName = purchasedGood.orgUnit.Name,
                                          ConsumptionStart = purchasedGood.prData.ConsumptionStart,
                                          ConsumptionEnd = purchasedGood.prData.ConsumptionEnd,
                                          TransactionDate = purchasedGood.prData.TransactionDate,
                                          Quantity = purchasedGood.prData.Quantity,
                                          UnitId = purchasedGood.unit.Id,
                                          UnitName = purchasedGood.unit.Name,
                                          ProductCode = purchasedGood.product.ProductCode,
                                          CO2eq = purchasedGood.productEmission != null ? purchasedGood.productEmission.CO2eq : null, //purchasedGood.emData != null ? purchasedGood.emData.CO2E : purchasedGood.product.CO2eq,
                                          CO2eqUnitId = purchasedGood.productEmission != null ? purchasedGood.productEmission.CO2eqUnitId : null //purchasedGood.emData != null ? purchasedGood.emData.CO2EUnitId : purchasedGood.product.CO2eqUnitId
                                      }).ToList();

                var result = new PagedResultDto<PurchasedProductVM>()
                {
                    Items = ObjectMapper.Map<List<PurchasedProductVM>>(purchasedGoods),
                    TotalCount = purchasedGoods.Count
                };
                return result;
            }
            catch (Exception exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Add Purchased Products Method
        /// </summary>
        /// <returns></returns>
        public async Task<PurchasedProductsData> AddPurchaseProductAsync(ActivityDataVM input)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                var activityType = _activityTypeRepository.GetAll()
                    .Include(x => x.EmissionsSource)
                    .FirstOrDefault(x => x.EmissionsSource.Id == input.EmissionSourceId);
                var activityId = Guid.NewGuid();

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                var activityModel = new PurchasedProductsData
                {
                    Id = activityId,
                    Name = input.Name,
                    Quantity = input.Quantity ?? 0,
                    UnitId = input.UnitId,
                    TransactionId = input.TransactionId,
                    TransactionDate = input.ConsumptionEnd ?? default(DateTime), //TODO: this may need to be reviewed and brought up to the UI, now seting to the end of the consumtion period.
                    ActivityTypeId = activityType?.Id,
                    Description = input.Description,
                    IsActive = true,
                    DataQualityType = DataQualityType.Actual,
                    IndustrialProcessId = 1,
                    ConsumptionStart = input.ConsumptionStart ?? default(DateTime),
                    ConsumptionEnd = input.ConsumptionEnd ?? default(DateTime),
                    OrganizationUnitId = input.OrganizationUnitId,
                    isProcessed = false,
                    ProductId = input.ProductId,
                    ProductCode = input.ProductCode,
                    EmissionGroupId = input.EmissionGroupId,
                    EmissionFactorId = input.EmissionFactorId,
                    Status = input.ActivityDataStatus,
                };

                var emissionsModel = new Emission
                {
                    OrganizationUnitId = input.OrganizationUnitId,
                    EmissionsDataQualityScore = GHG.EmissionsDataQualityScore.Estimated,
                    EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                    ResponsibleEntityID = input.ProductId,
                    ResponsibleEntityType = (int)GHG.ResponsibleEntityTypes.Product,
                    IsActive = true,
                    CO2E = calculatedEmissions,
                    CO2EUnitId = input.CO2eUnitId,
                    CO2eFactor = input.CO2e,
                    CO2eFactorUnitId = input.CO2eUnitId,
                    CreationTime = DateTime.UtcNow,
                    ActivityDataId = activityId
                };

                var result = await _purchaseProductsRepository.InsertAsync(activityModel);
                await _emissionsRepository.InsertAsync(emissionsModel);
                await CurrentUnitOfWork.SaveChangesAsync();

                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: AddPurchaseProductAsync - Exception: {ex}");
                return null;
            }


        }

        /// <summary>
        /// Update Purchased Products Method
        /// </summary>
        /// <returns></returns>
        public async Task<PurchasedProductsData> UpdatePurchaseProductAsync(ActivityDataVM input, Guid organizationId)
        {
            try
            {
                if (input.CO2eUnitId == 0) throw new UserFriendlyException("CO2e Unit Id cannot be 0");

                var purchasedProductData = _purchaseProductsRepository.Get(input.PurchasedProductId);
                var activityType = _activityTypeRepository.GetAll()
                    .Include(x => x.EmissionsSource)
                    .FirstOrDefault(x => x.EmissionsSource.Id == input.EmissionSourceId);

                float? calculatedEmissions = EmissionsCalculator.CalculateEmissions(input.Emission, input.CO2e, input.Quantity);

                purchasedProductData.Name = input.Name;
                purchasedProductData.Quantity = input.Quantity ?? 0;
                purchasedProductData.UnitId = input.UnitId;
                purchasedProductData.TransactionId = input.TransactionId;
                purchasedProductData.TransactionDate = input.ConsumptionEnd ?? default(DateTime);
                purchasedProductData.ActivityTypeId = activityType?.Id;
                purchasedProductData.Description = input.Description;
                purchasedProductData.IsActive = true;
                purchasedProductData.DataQualityType = DataQualityType.Actual;
                purchasedProductData.IndustrialProcessId = 1;
                purchasedProductData.ConsumptionStart = input.ConsumptionStart ?? default(DateTime);
                purchasedProductData.ConsumptionEnd = input.ConsumptionEnd ?? default(DateTime);
                purchasedProductData.OrganizationUnitId = input.OrganizationUnitId;
                purchasedProductData.isProcessed = false;
                purchasedProductData.ProductId = input.ProductId;
                purchasedProductData.EmissionGroupId = input.EmissionGroupId;
                purchasedProductData.EmissionFactorId = input.EmissionFactorId;
                purchasedProductData.Status = input.ActivityDataStatus;

                var emissionRow = _emissionsRepository
                    .GetAll()
                    .Include(e => e.ActivityData)
                    .Where(e => e.ActivityDataId == input.Id).FirstOrDefault();

                //TODO: As soon as the frontend behavior changes, refactor this to take in consideration the Unit entered by the User.
                //As is implemented right now, the user can choose the Unit for the CO2e, but from the frontend
                //the CO2e gets converted into kg and the unit gets saved as selected, therefore any checks regarding the Unit is useless.
                if (emissionRow != null)
                {
                    var emissionData = _emissionsRepository.Get(emissionRow.Id);
                    emissionData.CO2E = calculatedEmissions;
                    emissionData.CO2EUnitId = input.CO2eUnitId;
                    emissionData.CO2eFactor = input.CO2e;
                    emissionData.CO2eFactorUnitId = input.CO2eUnitId;
                    await _emissionsRepository.UpdateAsync(emissionData);
                }
                else
                {
                    var emissionsModel = new Emission
                    {
                        OrganizationUnitId = input.OrganizationUnitId,
                        EmissionsDataQualityScore = EmissionsDataQualityScore.Estimated,
                        EmissionsFactorsLibraryId = ClimateCampConsts.DefaultEmissionsFactorsLibraryId,
                        ResponsibleEntityID = Guid.Empty,
                        ResponsibleEntityType = (int)ResponsibleEntityTypes.User,
                        IsActive = true,
                        CO2E = calculatedEmissions,
                        CO2EUnitId = input.CO2eUnitId,
                        CreationTime = DateTime.UtcNow,
                        ActivityDataId = input.PurchasedProductId,
                        CO2eFactor = input.CO2e,
                        CO2eFactorUnitId = input.CO2eUnitId
                    };
                    await _emissionsRepository.InsertAsync(emissionsModel);
                }
                var result = await _purchaseProductsRepository.UpdateAsync(purchasedProductData);

                var customerProduct = await _customerProductRepository.FirstOrDefaultAsync(x => x.OrganizationId == organizationId && x.ProductId == input.ProductId);

                if(customerProduct != null)
                {
                    customerProduct.ProductCode = input.ProductCode;

                    await _customerProductRepository.UpdateAsync(customerProduct);
                }
                else
                {
                    var product = _productRepository.Get(input.ProductId);

                    var customerPrd = new CustomerProduct
                    {
                        Name = product.Name,
                        ProductCode = input.ProductCode,
                        OrganizationId = organizationId,
                        Description = product.Description,
                        ProductId = product.Id,
                        ImagePath = product.ImagePath,
                        UnitId = product.UnitId
                    };

                    await _customerProductRepository.InsertAsync(customerPrd);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                return result;
            }
            catch (UserFriendlyException userEx)
            {
                throw new UserFriendlyException(userEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: UpdatePurchaseProductAsync - Exception: {ex}");
                return null;
            }
        }


    }
}
