using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class EmissionGroupsVM
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? EmissionSourceId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? ParentEmissionGroupId { get; set; }
        public ICollection<EmissionGroupsVM> Children { get; set; }
    }
}
