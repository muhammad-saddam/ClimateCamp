using ClimateCamp.Common.Interfaces;
using System;

namespace ClimateCamp.Application
{
    public class GetAllDataCollectionsByOrganizationId : IMustHaveOrganization
    {
        public Guid OrganizationId { get; set; }
    }
}
