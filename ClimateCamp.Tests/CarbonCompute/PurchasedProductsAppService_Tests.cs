using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Validation;
using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Core;
using ClimateCamp.Core.CarbonCompute.Enum;
using ClimateCamp.Tests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Common.Tests.Users
{
    public class PurchasedProductsAppService_Tests : ClimateCampTestBase
    {
        private readonly IPurchasedProductsAppService _purchasedProductsAppService;
        private readonly ClimateCampTestData _climateCampTestData;
        private readonly IRepository<Organization, Guid> _organisationRepository;
        private readonly IRepository<User, long> _userRepository;

        public PurchasedProductsAppService_Tests()
        {
            //Creating the class which is tested (SUT - Software Under Test)
            //GetRequiredService
            _purchasedProductsAppService = Resolve<IPurchasedProductsAppService>();
            
            _organisationRepository = Resolve<IRepository<Organization, Guid>>();
            _userRepository = Resolve<IRepository<User, long>>();
            

            _climateCampTestData = Resolve<ClimateCampTestData>();
        }

        [Trait("Category", "Integration")]
        [Fact]
        public async Task Should_Add_PurchasedProducts_Test()
        {

            //Arrange - Prepare for test
            var initialCount = UsingDbContext(context => context.PurchaseProductsData.Count());
            PrepareCustomersSuppliersScenario();

            var organizationsCount = UsingDbContext(context => context.Organizations.Count());
            var usersCount = UsingDbContext(context => context.Users.Count());

            //Act -Run the SUT (software under test - the actual testing code)
            _ = _purchasedProductsAppService.AddPurchaseProductAsync(new ActivityDataVM
            {

                OrganizationUnitId = Guid.Empty,
                ProductId = _climateCampTestData.ProductMalt10Id,
                Quantity = 1,
                UnitId = GHG.UnitsEnum.kg.To<int>(),
                Name = "PurchaseProductsData test",
                Description = "",
                TransactionId = String.Empty,
                TransactionDate = new DateTime(2021, 1, 1),
                ActivityTypeId = 27,//Purchased products & services
                DataQualityType = DataQualityType.Actual,
                ConsumptionStart = new DateTime(2021, 1, 1),
                ConsumptionEnd = new DateTime(2021, 1, 1)
            });

            //Assert - Check results
            UsingDbContext(context =>
            {
                context.PurchaseProductsData.Count().ShouldBe(initialCount + 1);
                context.PurchaseProductsData.FirstOrDefault(t => t.Name == "PurchaseProductsData test").ShouldNotBe(null);
                var task2 = context.PurchaseProductsData.FirstOrDefault(t => t.Description == "my test task 2");
            });
        }

        /// <summary>
        /// expect that the method throws AbpValidationException if I don't set Name for creating task. 
        /// Because Description property is marked as Required in CreatePurchaseProductDto DTO class
        /// </summary>
        [Trait("Category", "Integration")]
        [Fact]
        public void Should_Not_Create_PurchasedProducts_Without_Description()
        {
            //Description is not set
            Assert.ThrowsAsync<AbpValidationException>(() => _purchasedProductsAppService.AddPurchaseProductAsync(new ActivityDataVM()));
        }

        private void PrepareCustomersSuppliersScenario()
        {

            UsingDbContext(context =>
            {

                var brewer = _organisationRepository.InsertAsync(new Organization
                {
                    Id = _climateCampTestData.OrganizationBrewerySuplierId,
                    Name = _climateCampTestData.OrganizationBrewerySuplierName,
                    Status = OrganizationStatus.Claimed.To<int>(),
                    IsActive = true,
                    TenantId = 1

                });

                var supplier = _organisationRepository.InsertAsync(new Organization
                {
                    Id = _climateCampTestData.OrganizationBreweryId,
                    Name = _climateCampTestData.OrganizationBreweryName,
                    Status = OrganizationStatus.Claimed.To<int>(),
                    IsActive = true,
                    TenantId = 1
                });

                context.SaveChanges();


                var dave = _userRepository.Insert(new User
                {
                    UserName = _climateCampTestData.UserDaveUserName,
                    EmailAddress = _climateCampTestData.UserDaveUserName,
                    Name = "test",
                    Surname = "test",
                    IsActive = true,
                    IsEmailConfirmed = true,
                    OrganizationId = _climateCampTestData.OrganizationBrewerySuplierId
                });
                dave.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(dave, ClimateCampConsts.DefaultPassPhrase);

                var lisa = _userRepository.Insert(new User
                {
                    UserName = _climateCampTestData.UserLisaUserName,
                    EmailAddress = _climateCampTestData.UserLisaUserName,
                    Name = "test",
                    Surname = "test",
                    IsActive = true,
                    IsEmailConfirmed = true,
                    OrganizationId = _climateCampTestData.OrganizationBreweryId
                });
                lisa.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(lisa, ClimateCampConsts.DefaultPassPhrase);

                context.SaveChanges();



            });
        }
    }
}
