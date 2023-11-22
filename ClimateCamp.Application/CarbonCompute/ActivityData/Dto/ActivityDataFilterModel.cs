using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Model used to filter activity data by OrganizationId and consumption start and end dates
    /// </summary>
    public class ActivityDataFilterModel
    {
        /// <summary>
        /// Consumption start date of activity data.
        /// </summary>
        public DateTime ConsumptionStart { get; set; }
        /// <summary>
        /// Consumption end date of activity data
        /// </summary>
        public DateTime ConsumptionEnd { get; set; }
        /// <summary>
        /// Organization Id
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// selected emission source
        /// </summary>
        public int EmissionSourceId { get; set; }
    }
}
