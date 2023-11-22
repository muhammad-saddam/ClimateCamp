using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IBusinessTravelAppService : IApplicationService
    {
        Task<BusinessTravelData> AddBusinessTravelDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetBusinessTravelData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<BusinessTravelData> UpdateBusinessTravelDataAsync(ActivityDataVM input);
    }
}