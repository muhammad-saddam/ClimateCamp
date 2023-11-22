using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IWasteGeneratedAppService : IApplicationService
    {
        Task<WasteGeneratedData> AddWasteGeneratedDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetWasteGeneratedData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<WasteGeneratedData> UpdateWasteGeneratedDataAsync(ActivityDataVM input);
    }
}
