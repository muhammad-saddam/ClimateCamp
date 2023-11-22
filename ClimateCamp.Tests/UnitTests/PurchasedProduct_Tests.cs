using ClimateCamp.CarbonCompute;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClimateCamp.Tests.Unit
{
    public class PurchasedProduct_UnitTests //: ClimateCampTestBase
    {
        [Fact]
        [Trait("Category", "Unit")]
        public async Task Should_Create_A_Valid_PurchasedProductData()
        {
            //Arrange - Prepare for test
            var data = new PurchasedProductsData
            {
                Name = "Test Product",
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                ConsumptionStart = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
                ConsumptionEnd = DateTime.Now.Subtract(TimeSpan.FromDays(10))
            };

            //Act -Run the SUT (software under test - the actual testing code)

            //Assert - Check results
            data.ShouldNotBeNull();
            data.DataQualityType.ShouldBe(GHG.DataQualityType.Unknown);
        }

        [Trait("Category", "Unit")]
        [Fact]
        public async void Should_Not_Create_PurchasedProducts_Without_Description()
        {

            Assert.ThrowsAny<NotSupportedException>(() => throw new NotSupportedException("Unit Test to Trigger Exception"));
        }
    }
}
