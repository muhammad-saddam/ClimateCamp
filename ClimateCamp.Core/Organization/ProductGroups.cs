using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    /// <summary>
    /// Storing grouped products data
    /// </summary>
    [Table("ProductGroups", Schema = "Master")]
    public class ProductGroups : Entity<Guid>, IPassivable
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }

        [ForeignKey(nameof(ParentProductGroupId))]
        public virtual ProductGroups ParentProductGroup { get; set; }
        public Guid? ParentProductGroupId { get; set; }

        public virtual ICollection<ProductGroups> Children { get; set; }
        public bool IsActive { get; set; }
    }
}
