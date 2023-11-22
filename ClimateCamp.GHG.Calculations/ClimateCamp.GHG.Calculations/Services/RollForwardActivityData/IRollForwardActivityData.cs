using ClimateCamp.Application;
using ClimateCamp.CarbonCompute;
using ClimateCamp.GHG.Calculations.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimateCamp.GHG.Calculations.Services.RollForwardActivityData
{
    public interface IRollForwardActivityData
    {
        Task<bool> RollForwardActivityDataByOrganizationId(Guid organizationId, DateTime consumptionStart, DateTime consumptionEnd, DateTime targetPeriodStart, DateTime targetPeriodEnd);
        Task<bool> HasNoActivityData(Guid organizationId, DateTime targetPeriodStart, DateTime targetPeriodEnd);
    }
}
