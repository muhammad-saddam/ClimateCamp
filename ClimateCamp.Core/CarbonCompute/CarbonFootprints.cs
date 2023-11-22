using Abp.Domain.Entities;
using ClimateCamp.Core.CarbonCompute.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    [Table("CarbonFootprints", Schema = "Transactions")]
    public class CarbonFootprints : Entity<Guid>
    {
        public virtual ProductEmissions ProductEmission { get; set; }

        /// <summary>
        /// The amount of 'Declared Units' contained within the product to which the PCF is referring to. The value MUST be strictly greater than 0. <br/>
        /// We are mapping the 'Declared Units' to <see cref="Product.UnitId"/> <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public double? UnitaryProductAmount { get; set; }

        /// <summary>
        /// The product carbon footprint of the product excluding biogenic emissions. <br/>
        /// The value MUST be calculated per declared unit with unit kg of CO2 equivalent per declared unit (kgCO2e / declaredUnit), expressed as a decimal equal to or greater than zero. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public double? PcfExcludingBiogenic { get; set; }

        /// <summary>
        /// If present, the product carbon footprint of the product including biogenic emissions. <br/>
        /// The value MUST be calculated per declared unit with unit kg of CO2 equivalent per declared unit (kgCO2e / declaredUnit), expressed as a decimal. <br/>
        /// Note: the value of this property can be less than 0 (zero). <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? PcfIncludingBiogenic { get; set; }

        /// <summary>
        /// The emissions from the combustion of fossil sources. <br/>
        /// The value MUST be calculated per declared unit with unit kg of CO2 equivalent per declared unit (kgCO2e / declaredUnit), expressed as a decimal equal to or greater than zero. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public double? FossilGhgEmissions { get; set; }

        /// <summary>
        /// The fossil carbon amount embodied in the product. <br/>
        /// The value MUST be calculated per declared unit with unit kg of CO2 equivalent per declared unit (kgCO2e / declaredUnit), expressed as a decimal equal to or greater than zero. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public double? FossilCarbonContent { get; set; }

        /// <summary>
        /// The biogenic carbon amount embodied in the product. <br/>
        /// The value MUST be calculated per declared unit with unit kg per declared unit (kg / declaredUnit), expressed as a decimal equal to or greater than zero. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public double? BiogenicCarbonContent { get; set; }

        /// <summary>
        /// If present, emissions resulting from recent (i.e., previous 20 years) carbon stock loss due to land conversion directly on the area of land under consideration. <br/>
        /// The value of this property MUST include direct land use change (dLUC) where available, otherwise statistical land use change (sLUC) can be used. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? DLucGhgEmissions { get; set; }

        /// <summary>
        /// If present, GHG emissions and removals associated with land-management-related changes, including non-CO2 sources. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? LandManagementGhgEmissions { get; set; }

        /// <summary>
        /// If present, all other biogenic GHG emissions associated with product manufacturing and transport that are not included in dLUC, iLuc, or land management. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? OtherBiogenicGhgEmissions { get; set; }

        /// <summary>
        /// If present, emissions resulting from recent (i.e., previous 20 years) carbon stock loss due to land conversion on land not owned or controlled by the company or in its supply chain, <br/>
        /// induced by change in demand for products produced or sourced by the company. <br/>
        /// PACT Required: Optional
        /// </summary>
        public double? ILucGhgEmissions { get; set; }

        /// <summary>
        /// If present, the Biogenic carbon content in the product converted to CO2e. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? BiogenicCarbonWithdrawal { get; set; }

        /// <summary>
        /// If present, the GHG emissions resulting from aircraft engine usage for the transport of the product. <br/>
        /// PACT Required: Optional
        /// </summary>
        public double? AircraftGhgEmissions { get; set; }

        /// <summary>
        /// The IPCC version of the GWP characterization factors used in the calculation of the PCF. <br/>
        /// AR6 or AR5. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string CharacterizationFactors { get; set; }

        // TODO: check if this is okay, not sure if this property should be able to store multiple predefined values or just one.
        /// <summary>
        /// The cross-sectoral standards applied for calculating or allocating GHG emissions. See <see href="https://wbcsd.github.io/data-exchange-protocol/v2/#crosssectoralstandardset"/> for more info. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public CrossSectoralStandards? CrossSectoralStandardsUsed { get; set; }

        // TODO: check if this is okay, technically the property should be a list of strings, but we are not sure if this is the correct way to do it.
        /// <summary>
        /// The product-specific or sector-specific rules applied for calculating or allocating GHG emissions. <br/>
        /// If no product or sector specific rules were followed, this set MUST be empty. <br/>
        /// PACT Required: Optional
        /// </summary>
        public string ProductOrSectorSpecificRules { get; set; }

        /// <summary>
        /// The standard followed to account for biogenic emissions and removals. <br/>
        /// If defined, the value MUST be one of the following: PEF, ISO, GHGP or Quantis. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public BiogenicAccountingMethodology? BiogenicAccountingMethodology { get; set; }

        /// <summary>
        /// The processes attributable to each lifecycle stage. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string BoundaryProcessesDescription { get; set; }

        /// <summary>
        /// The start of the time boundary for which the PCF value is considered to be representative. <br/>
        /// PAC Required: Mandatory
        /// </summary>
        public DateTime? ReferencePeriodStart { get; set; }

        /// <summary>
        /// The end of the time boundary for which the PCF value is considered to be representative. <br/>
        /// PAC Required: Mandatory
        /// </summary>
        public DateTime? ReferencePeriodEnd { get; set; }

        /// <summary>
        /// If present, a ISO 3166-2 Subdivision Code. <br/>
        /// Example value for the State of New York in the United States of America: 'US-NY'. <br/>
        /// Example value for the department Yonne in France: 'FR-89'. <br/>
        /// See <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#dt-carbonfootprint">PACT - Scope of a CarbonFootprint</a> for more information. <br/>
        /// PACT Required: Unknown.
        /// </summary>
        public string GeographyCountrySubdivision { get; set; }

        /// <summary>
        /// If present, the value MUST conform to data type ISO3166CC. <br/>
        /// Example value in case the geographic scope is France: 'FR'. <br/>
        /// See <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#dt-carbonfootprint">PACT - Scope of a CarbonFootprint</a> for more information. <br/>
        /// PACT Required: Unknown.
        /// </summary>
        public string GeographyCountry { get; set; }

        // TODO: check and see if this property should be an entity by itself, or an enum. PACT has a list of 22 regions or subregions defined in accordance with the UN M49 standard.
        /// <summary>
        /// If present, the value MUST conform to data type: <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#enumdef-regionorsubregion">RegionOrSubregion</a>. <br/>
        /// See <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#dt-carbonfootprint">PACT - Scope of a CarbonFootprint</a> and <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#elementdef-emissionfactordsset">PACT DataType: EmissionFactorDSSet</a> for more information. <br/>
        /// PACT Required: Unknown.
        /// </summary>
        public string GeographyRegionOrSubregion { get; set; }

        // TODO: check if this is okay, type is Array, Optional.
        /// <summary>
        /// If secondary data was used to calculate the CarbonFootprint, then it MUST include the property <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#element-attrdef-carbonfootprint-secondaryemissionfactorsources">secondaryEmissionFactorSources</a> <br/>
        /// with value the emission factors used for the CarbonFootprint calculation.<br/>
        /// PACT Required: Optional.
        /// </summary>
        public string SecondaryEmissionFactorSources { get; set; }

        /// <summary>
        /// The Percentage of emissions excluded from PCF, expressed as a decimal number between 0.0 and 5 including. <br/>
        /// PAC Required: Mandatory
        /// </summary>
        public decimal? ExemptedEmissionsPercent { get; set; }

        /// <summary>
        /// Rationale behind exclusion of specific PCF emissions, CAN be the empty string if no emissions were excluded. <br/>
        /// PAC Required: Mandatory
        /// </summary>
        public string ExemptedEmissionsDescription { get; set; }

        /// <summary>
        /// A boolean flag indicating whether packaging emissions are included in the PCF. <br/>
        /// PAC Required: Mandatory
        /// </summary>
        public bool? PackagingEmissionsIncluded { get; set; }

        /// <summary>
        /// Emissions resulting from the packaging of the product. <br/>
        /// The value MUST NOT be defined if <see cref="PackagingEmissionsIncluded"/> is false. <br/>
        /// PACT Required: Optional
        /// </summary>
        public double? PackagingGhgEmissions { get; set; }

        /// <summary>
        /// If present, a description of any allocation rules applied and the rationale explaining how the selected approach aligns with Pathfinder Framework rules (see Section 3.3.1.4). <br/>
        /// PACT Required: Optional
        /// </summary>
        public string AllocationRulesDescription { get; set; }

        /// <summary>
        /// If present, the results, key drivers, and a short qualitative description of the uncertainty assessment. <br/>
        /// PACT Required: Optional
        /// </summary>
        public string UncertaintyAssessmentDescription { get; set; }

        /// <summary>
        /// Percentage of PCF included in the data quality assessment based on the >5% emissions threshold. <br/>
        /// In PACT, this is part of the 'dqi' property, which is an Object, Optional*. <br/>
        /// PACT property name: 'coveragePercent'. <br/>
        /// PAC Required: Optional* (Mandatory after 2025)
        /// </summary>
        #region Dqi. PACT section 4.3 https://wbcsd.github.io/data-exchange-protocol/v2/#elementdef-dataqualityindicators
        public float? DqiCoveragePercent { get; set; }

        /// <summary>
        /// Quantitative data quality rating (DQR) based on the data quality matrix (See Pathfinder Framework Table 9), <br/>
        /// scoring the technological representativeness of the sources used for PCF calculation based on weighted average of all inputs representing >5% of PCF emissions. <br/>
        /// The value MUST be a decimal between 1 and 3 including.
        /// /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? TechnologicalDQR { get; set; }

        /// <summary>
        /// Quantitative data quality rating (DQR) based on the data quality matrix (Table 9), <br/>
        /// scoring the temporal representativeness of the sources used for PCF calculation based on weighted average of all inputs representing >5% of PCF emissions. />
        /// The value MUST be a decimal between 1 and 3 including. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? TemporalDQR { get; set; }

        /// <summary>
        /// Quantitative data quality rating (DQR) based on the data quality matrix (Table 9), <br/>
        /// scoring the geographical representativeness of the sources used for PCF calculation based on weighted average of all inputs representing >5% of PCF emissions. <br/>
        /// The value MUST be a decimal between 1 and 3 including. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? GeographicalDQR { get; set; }

        /// <summary>
        /// Quantitative data quality rating (DQR) based on the data quality matrix (Table 9), <br/>
        /// scoring the completeness of the data collected for PCF calculation based on weighted average of all inputs representing >5% of PCF emissions. <br/>
        /// The value MUST be a decimal between 1 and 3 including. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? CompletenessDQR { get; set; }

        /// <summary>
        /// Quantitative data quality rating (DQR) based on the data quality matrix (Table 9), <br/>
        /// scoring the reliability of the data collected for PCF calculation based on weighted average of all inputs representing >5% of PCF emissions. <br/>
        /// The value MUST be a decimal between 1 and 3 including. <br/>
        /// PACT Required: Optional* (Mandatory after 2025)
        /// </summary>
        public double? ReliabilityDQR { get; set; }
        #endregion
    }
}
