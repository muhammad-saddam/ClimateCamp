using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(ClimateCamp.CarbonCompute.EmissionGroups))]
    public class CreateEmissionGroupDto :EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid? OrganizationId { get; set; }
        public int? EmissionSourceId { get; set; }
        public Guid? ParentEmissionGroupId { get; set; }
    }
}
