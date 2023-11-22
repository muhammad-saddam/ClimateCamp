using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public interface IEmissionGroupsAppService
    {
        Task<PagedResultDto<EmissionGroupsDto>> GetAllGroupedEmissionsData(Guid organizationId);

        Task<bool?> CheckIfEmissionGroupRelatedToAnyActivityData(Guid groupId);

        Task InsertEmissionGroupsFromTemplate(string industryType, Guid organizationId);
    }
}
