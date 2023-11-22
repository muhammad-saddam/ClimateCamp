using Abp.Application.Services;
using ClimateCamp.PowerBI.Dto;
using ClimateCamp.PowerBI.Models;
using System.Threading.Tasks;

namespace ClimateCamp.PowerBI
{
    interface IPowerBIAppService : IApplicationService
    {
        Task<EmbedParams> GetPBIEmbedParams(GetPBIEmbedParamsDto input);
    }
}
