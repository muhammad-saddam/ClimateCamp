using Abp.Application.Editions;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class Organization : FullAuditedEntity<Guid>, IMustHaveTenant, IPassivable
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string VATNumber { get; set; }
        public Guid BillingPreferenceId { get; set; }
        public int? CountryId { get; set; }
        public string PictureName { get; set; }
        public string PicturePath { get; set; }
        public int BaseLineYear { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BaseLineEmission { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Revenue { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Target { get; set; }
        public int ReportingFrequencyId { get; set; }
        /// <summary>
        /// Start of the companies fiscal year
        /// </summary>
        public DateTime? AnnualReportingPeriod { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// By convention, each customer Organization of ClimateCamp represents a tenant of the platform.
        /// </summary>
        public int TenantId { get; set; }
        public int? TotalEmployees { get; set; }
        public int? EditionId { get; set; }
        public Guid? EmissionsFactorsLibraryId { get; set; }

        /// <summary>
        /// Identifier of the company registered in HubSpot
        /// </summary>
        public long? HubSpotId { get; set; }
        public long? ProductionQuantity { get; set; }
        public virtual Unit ProductionQuantityUnit { get; set; }

        public virtual Edition Editions { get; set; }
        /// <summary>
        /// Default emission factors library to be used in the emissions calculations
        /// </summary>
        public virtual EmissionsFactorsLibrary? EmissionsFactorsLibrary { get; set; }

        public virtual ICollection<OrganizationIndustry> OrganizationIndustries { get; set; }

        // public virtual ICollection<CustomerSupplier> CustomerOrganizations { get; set; }
        public virtual ICollection<CustomerSupplier> SupplierOrganizations { get; set; }
        /// <summary>
        /// Represents the Status of an Organization. The OrganizationStatus enum contains all the available statuses.
        /// </summary>
        [Required]
        public int Status { get; set; }
    }
}
