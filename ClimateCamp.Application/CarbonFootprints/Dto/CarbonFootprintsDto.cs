using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core.CarbonCompute.Enum;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(CarbonFootprints))]
    public class CarbonFootprintsDto : EntityDto<Guid>
    {
        public double? UnitaryProductAmount { get; set; }
        public double? PcfExcludingBiogenic { get; set; }
        public double? PcfIncludingBiogenic { get; set; }
        public double? FossilGhgEmissions { get; set; }
        public double? FossilCarbonContent { get; set; }
        public double? BiogenicCarbonContent { get; set; }
        public double? DLucGhgEmissions { get; set; }
        public double? LandManagementGhgEmissions { get; set; }
        public double? OtherBiogenicGhgEmissions { get; set; }
        public double? ILucGhgEmissions { get; set; }
        public double? BiogenicCarbonWithdrawal { get; set; }
        public double? AircraftGhgEmissions { get; set; }
        public string CharacterizationFactors { get; set; }
        public CrossSectoralStandards? CrossSectoralStandardsUsed { get; set; }
        public string ProductOrSectorSpecificRules { get; set; }
        public BiogenicAccountingMethodology? BiogenicAccountingMethodology { get; set; }
        public string BoundaryProcessesDescription { get; set; }
        public DateTime? ReferencePeriodStart { get; set; }
        public DateTime? ReferencePeriodEnd { get; set; }
        public string GeographyCountrySubdivision { get; set; }
        public string GeographyCountry { get; set; }
        public string GeographyRegionOrSubregion { get; set; }
        public string SecondaryEmissionFactorSources { get; set; }
        public decimal? ExemptedEmissionsPercent { get; set; }
        public string ExemptedEmissionsDescription { get; set; }
        public bool? PackagingEmissionsIncluded { get; set; }
        public double? PackagingGhgEmissions { get; set; }
        public string AllocationRulesDescription { get; set; }
        public string UncertaintyAssessmentDescription { get; set; }
        #region Dqi. PACT section 4.3 https://wbcsd.github.io/data-exchange-protocol/v2/#elementdef-dataqualityindicators
        public float? DqiCoveragePercent { get; set; }
        public double? TechnologicalDQR { get; set; }
        public double? TemporalDQR { get; set; }
        public double? GeographicalDQR { get; set; }
        public double? CompletenessDQR { get; set; }
        public double? ReliabilityDQR { get; set; }
        #endregion
    }
}
