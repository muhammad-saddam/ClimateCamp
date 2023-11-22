using ClimateCamp.Core.Editions;
using NSubstitute;
using Shouldly;
using System.Linq;

namespace ClimateCamp.Tests.Unit
{
    public class EditionManager_UnitTests
    {
        //[Fact]
        //[Trait("Category", "Unit")]
        public void Should_Create_A_Valid_Edition()
        {
            //Arrange - Prepare for test
            var em = Substitute.For<EditionManager>();
            var e = new CustomEdition
            {
                Name = EditionManager.DefaultEditionName,
                DisplayName = "Climatecamp Enterprise+ Edition",
                Image = "../../../assets/img/billings/billing-placeholder.png",
                PriceLabel = "Pricing On Request",
                IsContactSales = true
            };
            //Act -Run the SUT (software under test - the actual testing code)

            em.Create(e);

            //Assert - Verify the results
            em.Editions.Count().ShouldBe(1);
        }

    }
}
