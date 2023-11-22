using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IEndOfLifeTreatmentAppService : IApplicationService
    {
        Task<EndOfLifeTreatmentData> AddEndOfLifeTreatmentDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetEndOfLifeTreatmentData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<EndOfLifeTreatmentData> UpdateEndOfLifeTreatmentDataAsync(ActivityDataVM input);
    }
}
