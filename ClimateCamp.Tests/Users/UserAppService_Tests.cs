using ClimateCamp.Application;
using ClimateCamp.Common.Users;
using ClimateCamp.Common.Users.Dto;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace ClimateCamp.Tests.Users
{
    public class UserAppService_Tests : ClimateCampTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Trait("Category", "Integration")]
        [Fact]
        public async Task Initial_Data_Should_Contain_Admin_User()
        {
            //Act
            var result = await _userAppService.GetAllAsync(new PagedUserResultRequestDto());

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(u => u.UserName == ClimateCampConsts.AdminUserName || u.UserName == ClimateCampConsts.UserAdminUserName);
        }


        [Trait("Category", "Integration")]
        [Fact]
        public async Task Initial_Data_Should_Contain_Users()
        {
            // Act
            var output = await _userAppService.GetAllAsync(new PagedUserResultRequestDto());

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0, "No users in the database");
        }

        [Trait("Category", "Integration")]
        [Fact]
        public async Task Should_Create_User()
        {
            // Act
            await _userAppService.CreateAsync(
                new CreateUserDto
                {
                    EmailAddress = "john@volosoft.com",
                    IsActive = true,
                    Password = "123qwe",
                    UserName = "john.nash"
                });


            // Assert
            await UsingDbContextAsync(async context =>
            {
                var johnNashUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "john.nash");
                johnNashUser.ShouldNotBeNull();
            });
        }
    }
}
