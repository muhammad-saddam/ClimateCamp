using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class DataCollection : FullAuditedEntity<Guid>, IPassivable
    {
        public enum DataCollectionType
        {
            SecondaryData, PrimaryData
        }

        public DataCollectionType Type { get; set; }
        public string DataSourceName { get; set; }
        public string Category { get; set; }
        public string Channel { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}