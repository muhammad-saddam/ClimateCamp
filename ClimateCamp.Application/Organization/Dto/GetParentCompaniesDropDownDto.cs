using System;

namespace ClimateCamp.Application
{
    public class GetParentCompaniesDropDownDto
    {
        public Guid OrganizationId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
