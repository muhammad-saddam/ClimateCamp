using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{

    [Table("EmissionsSummaryScopeDetails", Schema = "Master")]
    public class EmissionsSummaryScopeDetails : Entity
    {
        /// <summary>
        /// Id of the organization unit's Emissions Summary
        /// </summary>
        [ForeignKey(nameof(EmissionSummaryId))]
        public Guid EmissionSummaryId { get; set; }
        /// <summary>
        /// Id of the relevant emission source from the GHG categories
        /// </summary>
        [ForeignKey(nameof(EmissionSourceId))]
        public int EmissionSourceId { get; set; }
        /// <summary>
        /// See '<see cref="CarbonCompute.Enum.Availability"/>' for the list of availability options.
        /// </summary>
        public int? Availability { get; set; }
        /// <summary>
        /// Tons CO2e for the specified emission source, organization unit and reference year.
        /// </summary>
        public float? tCO2e { get; set; }
        /// <summary>
        /// Tons CO2e per production unit for the specified emission source, organization unit and reference year.
        /// </summary>
        public float? tCO2ePPU { get; set; }
        /// <summary>
        /// See '<see cref="CarbonCompute.Enum.Scope3Methodology"/>' for the list of availability options.
        /// </summary>
        public int? Methodology { get; set; }
        /// <summary>
        /// Percentage of primary data for the specified emission source, organization unit and reference period
        /// </summary>
        public float? PrimaryDataShare { get; set; }
        /// <summary>
        /// Emission Scope Refers to Scope 1,2,3 values
        /// </summary>
        public int EmissionScope { get; set; }
        [NotMapped]
        public dynamic SelectedAvailability { get; set; }
        [NotMapped]
        public dynamic SelectedMethodology { get; set; }

    }


}
