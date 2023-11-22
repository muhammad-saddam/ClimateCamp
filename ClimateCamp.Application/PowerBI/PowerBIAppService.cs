using Abp.Authorization;
using ClimateCamp.PowerBI.Dto;
using ClimateCamp.PowerBI.Models;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.PowerBI
{
   
    public class PowerBIAppService : IPowerBIAppService
    {
        private readonly IPowerBIManager _powerBIManager;
        //need to move to env variables
        private Guid workspaceId = Guid.Parse("ca7e2e70-bdaa-4d6f-a1ae-6ed114aa7551");
        public PowerBIAppService(IPowerBIManager powerBIManager)
        {
            _powerBIManager = powerBIManager;
        }
        public async Task<EmbedParams> GetPBIEmbedParams(GetPBIEmbedParamsDto input)
        {
            return _powerBIManager.GetEmbedParams(input.ReportId, workspaceId);
        }
    }
}
