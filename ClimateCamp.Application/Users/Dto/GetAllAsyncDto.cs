using ClimateCamp.Common.Interfaces;
using System;

namespace ClimateCamp.Users.Dto
{
    public class GetAllAsyncDto : IMustHaveOrganization //PagedUserResultRequestDto,
    {
        public Guid OrganizationId { get; set; }
    }
}
