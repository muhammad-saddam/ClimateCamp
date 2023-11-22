using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Examples: https://climatecamp.sharepoint.com/sites/ProductDev/Shared%20Documents/General/08.%20Ecosystem/Microsoft/All%20Emission%20Factors%2011-9-2021%2010-43-28%20PM.xlsx?web=1
    /// Store the emission factors values in BaseUnits
    /// </summary>
    [Table("EmissionsFactors", Schema = "Reference")]
    public class EmissionsFactor : FullAuditedEntity<Guid>, IPassivable//, IMayHaveOrganizationUnit
    {
        public EmissionsFactor()
        {

        }
        public EmissionsFactor(EmissionsFactorsLibrary library, EmissionsSource emissionsSource, string name,
            string description, bool isActive, string documentationReference,
            float cO2, Unit cO2Unit, float n2O, Unit n2OUnit, float cH4, Unit cH4Unit, Unit Co2EUnitId)
        {
            this.Library = library;
            this.EmissionsSource = emissionsSource;
            this.Name = name;
            this.Description = description;
            this.IsActive = isActive;
            this.DocumentationReference = documentationReference;
            this.CO2 = cO2;
            this.CO2Unit = cO2Unit;
            this.CH4 = cH4;
            this.CH4Unit = cH4Unit;
            this.N20 = n2O;
            this.N20Unit = n2OUnit;
            this.CO2EUnit = Co2EUnitId;
        }

        public virtual EmissionsFactorsLibrary Library { get; set; }
        public virtual EmissionsSource EmissionsSource { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Sector Sector { get; set; }
        public int? SectorId { get; set; }
        #region Gas Factors
        public float CO2 { get; set; }
        [ForeignKey("CO2UnitId")]
        public Unit CO2Unit { get; set; }
        public float CH4 { get; set; }
        [ForeignKey("CH4UnitId")]
        public Unit CH4Unit { get; set; }
        public float N20 { get; set; }
        [ForeignKey("N20UnitId")]
        public Unit N20Unit { get; set; }
        public float HFCs { get; set; }
        [ForeignKey("HFCsUnitId")]
        public Unit HFCsUnit { get; set; }
        public float NF3 { get; set; }
        [ForeignKey("NF3UnitId")]
        public Unit NF3Unit { get; set; }

        public float PFCs { get; set; }
        [ForeignKey("PFCsUnitId")]
        public Unit PFCsUnit { get; set; }
        public float SF6 { get; set; }
        [ForeignKey("SF6UnitId")]
        public Unit SF6Unit { get; set; }
        public float CO2E { get; set; }
        [ForeignKey("CO2EUnitId")]
        public Unit CO2EUnit { get; set; }
        public float OtherGHGs { get; set; }
        [ForeignKey("OtherGHGsUnitId")]
        public Unit OtherGHGsUnit { get; set; }
        #endregion

        public bool IsActive { get; set; }

        /// <summary>
        /// Link or title of the referece document
        /// </summary>
        public string DocumentationReference { get; set; }

        [Obsolete("Conceptionally this modelling link to an OU is not needed, the factors are grouped in libraries who are then attacehd to a Org or calculation method.")]
        public virtual OrganizationUnit OrganizationUnit { get; set; }

        public LifeCycleActivity LifeCycleActivity { get; set; }

        /// <summary>
        /// ar4, ar5, ar6
        /// </summary>
        public string CalculationMethod { get; set; }

        /// <summary>
        /// source
        /// </summary>
        public string CalculationOrigin { get; set; }

        /// <summary>
        /// Unit for the activity activity rate
        /// </summary>
        [ForeignKey("UnitId")]
        public virtual Unit? Unit { get; set; }
        //https://github.com/climatiq/Open-Emission-Factors-DB/blob/main/DATA_GUIDANCE.md#region
        //public string Region { get; set; }
    }



}
