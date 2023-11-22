using System;

namespace ClimateCamp.Core
{
    public class OrganizationIndustry
    {
        public Guid OrganizationId { get; set; }
        public int IndustryId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Industry Industry { get; set; }

        public bool isPrimary { get; set; }
    }
}
