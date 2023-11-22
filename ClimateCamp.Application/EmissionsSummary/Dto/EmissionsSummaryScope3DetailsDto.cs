using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(EmissionsSummaryScopeDetails))]
    public class EmissionsSummaryScopeDetailsDto : EntityDto<int>
    {
        /// <summary>
        /// Id of the organization unit's Emissions Summary
        /// </summary>
        public Guid EmissionSummaryId { get; set; }
        /// <summary>
        /// Id of the relevant emission source from the GHG categories
        /// </summary>
        public int EmissionSourceId { get; set; }
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.Availability"/>' for the list of availability options.
        /// </summary>
        public int Availability { get; set; }
        /// <summary>
        /// Tons CO2e for the specified emission source, organization unit and reference year.
        /// </summary>
        public double tCO2e { get; set; }
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.Scope3Methodology"/>' for the list of availability options.
        /// </summary>
        public int Methodology { get; set; }
        /// <summary>
        /// Percentage of primary data for the specified emission source, organization unit and reference period
        /// </summary>
        public double PrimaryDataShare { get; set; }
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