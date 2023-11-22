using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ClimateCamp.Application
{
    public interface ITransportAndDistributionAppService : IApplicationService
    {
        Task<TransportAndDistributionData> AddTransportDataAndEmissionsAsync(ActivityDataVM input);
        Task<List<TransportationDataVM>> GetTransportationData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<TransportAndDistributionData> UpdateTransportDataAndEmissionsAsync(ActivityDataVM input);
        Task<PagedResultDto<VehicleTypesDto>> GetAllVehiclesByModeOfTransport(int modeOfTransport);
    }
}
