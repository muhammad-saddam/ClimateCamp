using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IFuelAndEnergyAppService : IApplicationService
    {
        Task<FuelAndEnergyData> AddFuelAndEnergyDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetFuelAndEnergyData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<FuelAndEnergyData> UpdateFuelAndEnergyDataAsync(ActivityDataVM input);

    }
}
