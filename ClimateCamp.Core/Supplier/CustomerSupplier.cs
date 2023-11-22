using Abp.Domain.Entities.Auditing;
using ClimateCamp.Lookup;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    [Table("CustomerSuppliers", Schema = "Transactions")]
    public class CustomerSupplier : AuditedEntity<long>
    {
        public Guid CustomerOrganizationId { get; set; }

        public Guid SupplierOrganizationId { get; set; }

        [ForeignKey(nameof(CustomerOrganizationId))]
        public virtual Organization CustomerOrganization { get; set; }

        [ForeignKey(nameof(SupplierOrganizationId))]
        public virtual Organization SupplierOrganization { get; set; }

        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }

        public string ContactEmailAddress { get; set; }

        public string Product { get; set; }

        /// <summary>
        /// Tons of purchased products yearly
        /// </summary>
        public long? YearlyProductQuantity { get; set; }

        public string Service { get; set; }

        /// <summary>
        /// Yearly invoiced services in €
        /// </summary>
        public long? YearlyServiceQuantity { get; set; }

        /// <summary>
        /// Tons of yearly CO2E for the delivered products and services
        /// </summary>
        [Obsolete("Not used currently, could be used as an agregates or a benchmark value when activity data is not available yet", false)]
        public float? CO2E { get; set; }

        /// <summary>
        /// ex. Food & Beverages/Brewing
        /// </summary>
        public string Industry { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// Represents the Relationship status between two Organizations (Customer/Supplier).<br/>
        /// Invited, Validated, Activated, Scored
        /// </summary>
        [Obsolete("Minimal to no use at the moment. Was decided in Task #855 to mark it as Obsolete rather than deleting it.", false)]
        public SupplierActivationStatus Status { get; set; }

        /// <summary>
        /// Country of origin
        /// </summary>
        /// 
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        public int? CountryId { get; set; }

    }
}
