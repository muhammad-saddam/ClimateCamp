using Abp.Application.Services;
using ClimateCamp.Common.Sessions.Dto;
using System.Threading.Tasks;

namespace ClimateCamp.Common.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
