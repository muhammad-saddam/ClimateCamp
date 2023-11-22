using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{

    /// <summary>
    /// Storing independant targets for organization units
    /// </summary>
    [Table("TargetIndependant", Schema = "Master")]
    public class TargetIndependant : Entity<Guid>
    { 
        public int BaseLineYear { get; set; }
        public int? TargetYear { get; set; }
        public float? Scope1Target { get; set; }
        public float? Scope2Target { get; set; }
        public float? Scope3Target { get; set; }
        public Guid OrganizationUnitId { get; set; }
        public virtual OrganizationUnit OrganizationUnit { get; set; }
        public Guid OrganizationTargetId { get; set; }
        public virtual OrganizationTarget OrganizationTarget { get; set; }
    }
}
