using Abp.Domain.Entities;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Storing grouped emission sources
    /// </summary>
    [Table("EmissionGroups", Schema = "Reference")]
    public class EmissionGroups : Entity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Icon { get; set; }
        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }
        public int? EmissionSourceId { get; set; }
        [ForeignKey(nameof(EmissionSourceId))]
        public EmissionsSource EmissionsSource { get; set; }
        public Guid? ParentEmissionGroupId { get; set; }
        [ForeignKey(nameof(ParentEmissionGroupId))]
        public EmissionGroups  ParentEmissionGroup { get; set; }
        public ICollection<EmissionGroups> Children { get; set; }
    }
}
