using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// table to store product emissions based on different reporting periods
    /// </summary>
    [Table("ProductEmissions", Schema = "Transactions")]
    public class ProductEmissions : CreationAuditedEntity<Guid>
    {
        public float? CO2eq { get; set; }
        /// <summary>
        /// PACT info: For reporting periods ending before 2025 and if this property is present, the property dqi MUST NOT be defined.
        /// </summary>
        public float? PrimaryDataShare { get; set; }
        public int? InventoryType { get; set; }
        public bool? Audited { get; set; }
        public string Certificate { get; set; }
        public string Auditor { get; set; }
        public int? Year { get; set; }
        public int? Period { get; set; }
        public int? PeriodType { get; set; }
        /// <summary>
        /// will act as an identifier when product will be shared from collaboration section
        /// rather the emission is product or organization emission
        /// </summary>
        public int? EmissionSourceType { get; set; }
        public bool? IsSelected { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product {get;set;}
        public Guid? OrganizationUnitId { get; set; }
        public virtual OrganizationUnit OrganizationUnit { get; set; }
        public Guid? CustomerOrganizationId { get; set; }
        [ForeignKey(nameof(CustomerOrganizationId))]
        public virtual Organization Organization { get; set; }
        public int? CO2eqUnitId { get; set; }
        [ForeignKey(nameof(CO2eqUnitId))]
        public virtual Unit Unit { get; set; }
        public ICollection<ProductsEmissionSources> ProductsEmissionSources { get; set; }

        #region PACT ProductFootprint
        /// <summary>
        /// The version of the ProductFootprint data specification with value 2.0.1-20230522. <br/>
        /// Subsequent revisions will update this value according to Semantic Versioning 2.0.0. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string SpecVersion { get; set; }

        /// <summary>
        /// The version of the ProductFootprint with value an integer in the inclusive range of 0..2^31-1. <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public int? Version { get; set; }

        /// <summary>
        /// Each ProductFootprint MUST include the property status with value one of the following values: <br/>
        /// Active or Deprecated. <br/>
        /// PACT property name: 'status', <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// If defined, the value should be a message explaining the reason for the current status(<see cref="IsActive"/>). <br/>
        /// 
        /// </summary>
        public string StatusComment { get; set; }

        /// <summary>
        /// The additional information related to the product footprint. <br/>
        /// Whereas the property 'productDescription' contains product-level information, comment SHOULD be used for information and instructions related to the calculation of the footprint, <br/>
        /// or other information which informs the ability to interpret, to audit or to verify the Product Footprint. <br/>
        /// PACT property name: 'comment' <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string ProductFootprintComment { get; set; }

        // TODO: add possible link to CompanyIds entity. This entity could store vendor and buyer assigned ids
        /// <summary>
        /// Gets or sets the ID of the linked CarbonFootprint entity.
        /// </summary>
        public Guid? CarbonFootprintId { get; set; }
        [ForeignKey(nameof(CarbonFootprintId))]
        public virtual CarbonFootprints CarbonFootprint { get; set; }


        #region Assurance (Audit). PACT section 4.4 https://wbcsd.github.io/data-exchange-protocol/v2/#elementdef-assurance
        /// <summary>
        /// The date at which the assurance was completed. <br/>
        /// PACT property name: 'completedAt' <br/>
        /// PACT Required: Optional
        /// </summary>
        public DateTime? AuditDate { get; set; }

        /// <summary>
        /// A ProductFootprint SHOULD include the property updated with value the timestamp of the ProductFootprint update. <br/>
        /// A ProductFootprint MUST NOT include this property if an update has never been performed. <br/>
        /// PACT property name: 'updated' <br/>
        /// PACT Required: Optional
        /// </summary>
        public DateTime? AuditUpdateDate { get; set; }

        // TODO: check if this could be better of as an enum, tking in consideration there are only 4 values. 
        /// <summary>
        /// Level of granularity of the emissions data assured, with value equal to: <br/>
        /// Corporate level, Product line, PCF system or Product level. <br/>
        /// This property MAY be undefined only if the kind of assurance was not performed. <br/>
        /// PACT property name: 'coverage'. <br/>
        /// PACT required: Optional
        /// </summary>
        public string AuditCoverage { get; set; }

        /// <summary>
        /// Level of assurance applicable to the PCF, with value equal to: <br/>
        /// <em>Limited</em> for limited assurance or <em>Reasonable</em> for reasonable assurance. <br/>
        /// This property MAY be undefined only if the kind of assurance was not performed. <br/>
        /// PACT property name: 'level'. <br/>
        /// PACT required: Optional
        /// </summary>
        public string AuditLevel { get; set; }

        /// <summary>
        /// Name of the standard against which the PCF was assured. <br/>
        /// PACT property name: 'standardName'. <br/>
        /// PACT required: Optional
        /// </summary>
        public string AuditStandardName { get; set; }

        /// <summary>
        /// Any additional comments that will clarify the interpretation of the assurance. <br/>
        /// This value of this property MAY be the empty string. <br/>
        /// PACT property name: 'comments'. <br/>
        /// PACT required: Optional
        /// </summary>
        public string AuditComments { get; set; }
        #endregion

        /// <summary>
        /// If defined, the start of the validity period of the ProductFootprint. <br/>
        /// The validity period is the time interval during which the ProductFootprint is declared as valid for use by a receiving data recipient. <br/>
        /// The validity period is defined by the properties '<see cref="ValidityPeriodStart"/>' (including) and '<see cref="ValidityPeriodEnd"/>' (excluding). <br/>
        /// If no validity period is specified, the ProductFootprint is valid for 3 years starting with '<see cref="CarbonFootprint.ReferencePeriodEnd"/>'. <br/>
        /// More details on the rules regarding the validity periods can be found <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#element-attrdef-productfootprint-validityperiodstart">here</a>. <br/>
        /// PACT required: Optional
        /// </summary>
        public DateTime? ValidityPeriodStart { get; set; }

        /// <summary>
        /// The end (excluding) of the valid period of the ProductFootprint. See '<see cref="ValidityPeriodStart"/>' for further details. <br/>
        /// PACT required: Optional
        /// </summary>
        public DateTime? ValidityPeriodEnd { get; set; }
        #endregion
    }
}
