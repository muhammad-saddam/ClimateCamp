using Abp.Domain.Services;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.Domain
{
    public interface IActivityDataManager : IDomainService
    {
        Task<ActivityData> GetActivityDataAsync(Guid id);

        Task CreateActivityDataAsync(ActivityData data);

        Task<IReadOnlyList<ActivityData>> GetOrganizationUnitActivityDataAsync(Guid ouId);

        Task<IReadOnlyList<ActivityData>> GetChildActivityDataAsync(Guid ouId);

        Task<IReadOnlyList<EmissionsFactor>> GetRegisteredUsersAsync(EmissionsFactorsLibrary library);
    }
}
