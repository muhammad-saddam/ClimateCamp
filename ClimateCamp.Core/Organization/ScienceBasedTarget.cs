using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    /// <summary>
    /// Storing science based targets for organization
    /// </summary>
    [Table("ScienceBasedTargets", Schema = "Master")]
    public class ScienceBasedTarget : Entity<Guid>
    {
        public int? BaseLineYear { get; set; }
        public int? SBTI { get; set; }
        public int? NearTermTarget { get; set; }
        public int? NearTermTargetYear { get; set; }
        public int? LongTermTarget { get; set; }
        public int? LongTermTargetYear { get; set; }
        public int? NetZeroCommitted { get; set; }
        public int? NetZeroYear { get; set; }
        public Guid OrganizationTargetId { get; set; }
        public virtual OrganizationTarget OrganizationTarget { get; set; }
    }
}
