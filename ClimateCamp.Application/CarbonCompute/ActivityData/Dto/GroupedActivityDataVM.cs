using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Model used to display activitieslist with grouped by emission group Id
    /// </summary>
    public class GroupedActivityDataVM
    {
        /// <summary>
        /// Emission Group Id
        /// </summary>
        public Guid EmissionGroupId { get; set; }
        /// <summary>
        /// Emission Group Name
        /// </summary>
        public string EmissionGroupName { get; set; }
        /// <summary>
        /// ParentEmissionGroupId
        /// </summary>
        public Guid? ParentEmissionGroupId { get; set; }
        /// <summary>
        /// Child Emission Groups
        /// </summary>
        public List<GroupedActivityDataVM> Children { get; set; }
        /// <summary>
        /// Activity Data with corresponding EmissionGroupId
        /// </summary>
        public List<ActivityDataVM> Activities { get; set; }

    }
}

