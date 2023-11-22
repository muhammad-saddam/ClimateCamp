using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{

    [Table("ActivityData", Schema = "Transactions")]
    public class ActivityData : FullAuditedEntity<Guid, User>, IPassivable//, IMustHaveOrganizationUnit, //, AggregateRoot<Guid> //to respect the DDD convention
    {

        /// <summary>
        /// https://aspnetboilerplate.com/Pages/Documents/Zero/Organization-Units
        /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.Zero.Common/Organizations/OrganizationUnit.cs
        /// </summary>
        [Obsolete("UserStory #918. OrganizationUnitId must be always specified in order to attribute accurately the activity and the corresponding emissions to the part of the company.")]
        public Guid? OrganizationUnitId { get; set; }
        public OrganizationUnit OrganizationUnit { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Distance Activity, Fuel Usage Activity
        /// </summary>
        public virtual ActivityType ActivityType { get; set; }

        public int? ActivityTypeId { get; set; }


        public bool IsActive { get; set; }

        public GHG.DataQualityType DataQualityType { get; set; }

        public DateTime ConsumptionStart { get; set; }
        public DateTime ConsumptionEnd { get; set; }
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Measurable unit (may correspond to kWh, Volume and others)
        /// </summary>
        public float Quantity { get; set; }

        public virtual Unit Unit { get; set; }
        public int? UnitId { get; set; }
        
        /// <summary>
        /// TODO: domain menaing of this property to be clearified.
        /// </summary>
        public int IndustrialProcessId { get; set; }
        
        public bool isProcessed { get; set; }

        /// <summary>
        /// Customer organization's specific identification of transaction responsible for the activity.
        /// Values may include invoice numbers, payment transactions, booking numbers, professional service intervention and similar.
        /// Is used for deduplication of activity data within the same <see cref="Organization"/>.
        /// TODO: There is an apparent duplication in semantics with the property <see cref="ActivityData.SourceTransactionId"/> and needs to be explicitly distinguished.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Holds a description, reference or identifier of any supporting evidence that proves the authenticity of the activity.
        /// </summary>
        public string Evidence { get; set; }

        /// <summary>
        /// Customer organization's specific identification of the process or activity.
        /// Is used for deduplication of activity data within the same <see cref="Organization"/>.
        /// TODO: There is an apparent duplication in semantics with the property <see cref="ActivityData.TransactionId"/> and needs to be explicitly distinguished.
        /// </summary>
        public string SourceTransactionId { get; set; }

        /// <summary>
        /// Holds the identifier of the data connector that ingested the activity data.
        /// </summary>
        public string ConnectorId { get; set; }

        /// <summary>
        /// Holds the emission group id assigned to the particular emission
        /// </summary>
        public Guid? EmissionGroupId { get; set; }
        [ForeignKey(nameof(EmissionGroupId))]
        public EmissionGroups EmissionGroup { get; set; }

        /// <summary>
        /// Holds the emission factor id
        /// </summary>
        public Guid? EmissionFactorId { get; set; }
        [ForeignKey(nameof(EmissionFactorId))]
        public EmissionsFactor EmissionFactor { get; set; }
        /// <summary>
        /// activity data status:
        /// <see cref="Core.CarbonCompute.Enum.ActivityDataStatus"/>
        /// </summary>
        public int Status { get; set; }
    }


}
