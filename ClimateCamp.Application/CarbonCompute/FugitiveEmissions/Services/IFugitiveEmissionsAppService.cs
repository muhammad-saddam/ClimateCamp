using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IFugitiveEmissionsAppService : IApplicationService
    {
        Task<FugitiveEmissionsData> AddFugitiveEmissionsDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetFugitiveEmissionsData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<List<GreenhouseGasGroup>> GetGroupedGreenhouseGasses();
        Task<FugitiveEmissionsData> UpdateFugitiveEmissionsDataAsync(ActivityDataVM input);
    }
}
