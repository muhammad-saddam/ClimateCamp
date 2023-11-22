using System;

namespace ClimateCamp.GHG.Calculations.Helpers
{
    public static class RollForwardConsumptionDateUpdateHelper
    {
        public static (DateTime, DateTime) UpdateConsumptionDates(DateTime sourcePeriodStart, DateTime targetPeriodStart, DateTime activityPeriodStart, DateTime activityPeriodEnd)
        {
            int months = (targetPeriodStart.Year - sourcePeriodStart.Year) * 12 + targetPeriodStart.Month - sourcePeriodStart.Month;
            activityPeriodStart = activityPeriodStart.AddMonths(months);
            activityPeriodEnd = activityPeriodEnd.AddMonths(months);
            return (activityPeriodStart, activityPeriodEnd);
        }
    }
}
