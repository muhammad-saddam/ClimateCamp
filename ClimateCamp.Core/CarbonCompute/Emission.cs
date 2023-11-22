using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

using ClimateCamp.Core;
using Microsoft.PowerBI.Api.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Storing calculated emissions for visualisation and report generation
    /// </summary>
    [Table("Emissions", Schema = "Transactions")]
    public class Emission : AuditedEntity<Guid>, IPassivable//, AggregateRoot<Guid> //to respect the DDD convention
    {
        [Obsolete("See user story #918")]
        public Guid? OrganizationUnitId { get; set; }
        [Obsolete("See user story #918")]
        public OrganizationUnit OrganizationUnit { get; set; }
        /// <summary>
        /// Fugitive, Combustion
        /// </summary>
        /// <summary>
        public Guid ActivityDataId { get; set; }
        public virtual ActivityData ActivityData { get; set; }
        public Guid EmissionsFactorsLibraryId { get; set; }
        public virtual EmissionsFactorsLibrary EmissionsFactorsLibrary { get; set; }
        public GHG.EmissionsDataQualityScore? EmissionsDataQualityScore { get; set; }
        /// <summary>
        /// Generic field to be able to capture the customer specific identification of a party (who, what thing, what process) is responsible for the emissions produced by the related activity. 
        /// As of this writing, there are no constraints on what is captured in this property, rather a placeholderfor future proofing the domain model.
        /// </summary>
        public Guid ResponsibleEntityID { get; set; }
        /// <summary>
        /// Represents a generic categorization of responsible party of the produced emissions.
        /// Currently used together with the enumeration <see cref="GHG.ResponsibleEntityTypes"/>
        /// </summary>
        public int ResponsibleEntityType { get; set; }
        
        public bool IsActive { get; set; }

        #region Individual Greenhouse Gas values
        public float? CO2 { get; set; }
        public Unit? CO2Unit { get; set; }
        public float? CH4 { get; set; }
        public Unit? CH4Unit { get; set; }
        public float? N20 { get; set; }
        public Unit? N20Unit { get; set; }
        public float? HFCs { get; set; }
        public Unit? HFCsUnit { get; set; }
        public float? NF3 { get; set; }
        public Unit? NF3Unit { get; set; }
        public float? PFCs { get; set; }
        public Unit? PFCsUnit { get; set; }
        public float? SF6 { get; set; }
        public Unit? SF6Unit { get; set; }

        /// <summary>
        /// The total emissions quantity expressed as CO2 equivalent value, incuding all the greenhouse gases.
        /// </summary>
        public float? CO2E { get; set; }
        /// <summary>
        /// Navigation property to the <see cref="Unit"/> that represent the unit in which the total quantity of <see cref="Emission.CO2E"/> gas.
        /// By convention the defacult is expected in kilograms (kg).
        /// </summary>
        public virtual Unit? CO2EUnit { get; set; }
        /// <summary>
        /// Identifier of the <see cref="Unit"/> entity <see cref="Emission.CO2EUnit"/> 
        /// </summary>
        public int? CO2EUnitId { get; set; }

        public float? OtherGHGs { get; set; }
        public Unit? OtherGHGsUnit { get; set; }
        #endregion
        public float? CO2eFactor { get; set; }
        public int? CO2eFactorUnitId { get; set; }
        public int Version { get; set; }
    }
}
