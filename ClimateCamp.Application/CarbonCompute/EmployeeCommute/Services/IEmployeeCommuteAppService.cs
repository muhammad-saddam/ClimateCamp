using Abp.Application.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IEmployeeCommuteAppService : IApplicationService
    {
        Task<EmployeeCommuteData> AddEmployeeCommuteDataAsync(ActivityDataVM input);
        Task<List<ActivityDataVM>> GetEmployeeCommuteData(Guid organizationId, int emissionSourceId, DateTime? consumptionStart, DateTime? consumptionEnd);
        Task<EmployeeCommuteData> UpdateEmployeeCommuteDataAsync(ActivityDataVM input);
        Task<List<VehicleTypeGroup>> GetGroupedVehicleTypes(int transportationKind);
    }
}