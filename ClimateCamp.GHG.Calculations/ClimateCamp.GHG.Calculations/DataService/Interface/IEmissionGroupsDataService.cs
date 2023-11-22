using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.DataService.Interface
{
    public interface IEmissionGroupsDataService
    {
        Task<List<EmissionGroups>> GetEmissionGroupsByOrganizationId(Guid organizationId);
        Task<List<GroupedEmissionsVM>> GetAllEmissionGroupsAndDataByOrganizationId(Guid organizationId, DateTime consumptionStart, DateTime consumptionEnd);
    }
}
