using Abp.Domain.Entities;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    public class Industry : Entity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<OrganizationIndustry> OrganizationIndustries { get; set; }
        public int? ParentIndustryId { get; set; }
        [ForeignKey(nameof(ParentIndustryId))]
        public Industry ParentIndustry { get; set; }
        public ICollection<Industry> Children { get; set; }
        public bool IsPriority { get; set; }
        [ForeignKey(nameof(DefaultUnitId))]
        public virtual Unit Unit { get; set; }
        public int? DefaultUnitId { get; set; }
    }
}
