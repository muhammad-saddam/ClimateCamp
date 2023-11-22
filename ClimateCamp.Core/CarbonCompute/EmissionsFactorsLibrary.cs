using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// US EPA, Climate Registry Default Emission Factors, UK Department for Environment Food & Rural Affairs(DEFRA) => (Business, Energy & Industrial Strategy) BEIS, ADEME
    /// </summary>
    [Table("EmissionsFactorsLibraries", Schema = "Reference")]
    public class EmissionsFactorsLibrary : FullAuditedEntity<Guid>, IMayHaveTenant, IPassivable//, AggregateRoot<Guid> //to respect the DDD convention
    {

        public EmissionsFactorsLibrary(int? tenantId, string name, int version, bool isActive, int year = 2019)
        {
            this.TenantId = tenantId;
            this.Name = name;
            this.Version = version;
            this.Year = year;
            this.IsActive = isActive;
        }

        /// <summary>
        /// When null, the libraries are shared across tenants and are maintained by host administrators.
        /// </summary>
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }

        public string Region { get; set; }
    }
}
