using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    /// <summary>
    /// Storing organization targets
    /// </summary>
    [Table("OrganizationTargets", Schema = "Master")]
    public class OrganizationTarget : Entity<Guid>
    {
        public int TSFType { get; set; }
        public Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
