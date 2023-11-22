using ClimateCamp.Common.Interfaces;
using System;

namespace ClimateCamp.Application
{
    public class GetAllAsyncWithChildrenDto : PagedCompanyResultRequestDto, IMustHaveOrganization
    {
        public Guid OrganizationId { get; set; }
    }
}
