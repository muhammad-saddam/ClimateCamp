using ClimateCamp.Application;
using ClimateCamp.Common.Users.Dto;
using Microsoft.Graph;
using System.Threading.Tasks;
namespace ClimateCamp.Infrastructure.AzureADB2C
{
    public interface IGraphClientService
    {
        Task<IGraphServiceUsersCollectionPage> ListUsers();
        Task<User> GetUserById(string id);
        Task<bool> DeleteUserById(string id);
        Task<User> SetPasswordByUserId(string userId, string Password);
        Task<User> CreateUser(CreateUserDto userDto);
        Task<User> UpdateUser(UserDto input);
        Task<User> GetUserBySignInName(string userId);
    }
}
