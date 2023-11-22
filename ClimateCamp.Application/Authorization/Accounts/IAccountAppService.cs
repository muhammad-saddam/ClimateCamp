using Abp.Application.Services;
using ClimateCamp.Common.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace ClimateCamp.Core.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
