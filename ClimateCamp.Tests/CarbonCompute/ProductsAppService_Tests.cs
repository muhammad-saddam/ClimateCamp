using ClimateCamp.Application;
using ClimateCamp.Tests;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClimateCamp.Common.Tests.Integration
{
    public class ProductsAppService_Tests : ClimateCampTestBase
    {
        private readonly IProductsAppService _productsAppService;
        private readonly ClimateCampTestData _climateCampTestData;
        private readonly IPurchasedProductsAppService _purchasedProductsAppService;


        public ProductsAppService_Tests()
        {
            //Creating the class which is tested (SUT - Software Under Test)
            _productsAppService = Resolve<IProductsAppService>();
            _purchasedProductsAppService = Resolve<IPurchasedProductsAppService>();

            //LocalIocManager.Register<ClimateCampTestData>(Abp.Dependency.DependencyLifeStyle.Singleton);
            _climateCampTestData = Resolve<ClimateCampTestData>();

        }


        /// <summary>
        /// expect that the method throws AbpValidationException if I don't set Name for creating task. 
        /// Because Description property is marked as Required in CreatePurchaseProductDto DTO class
        /// </summary>
        [Trait("Category", "Integration")]
        [Fact]
        public async Task Should_Retrieve_ProductsAccessRequest_OncePerProduct_Organization()
        {
            //Arrange

            Guid orgId = Guid.Empty;

            // Act
            var output = await _productsAppService.GetAllProductsManagementRequestData(orgId);

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }


    }
}
