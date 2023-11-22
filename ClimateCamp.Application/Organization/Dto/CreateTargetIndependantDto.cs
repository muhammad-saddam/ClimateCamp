using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(TargetIndependant))]
    public class CreateTargetIndependantDto : EntityDto<Guid>
    {
        public int BaseLineYear { get; set; }
        public int? TargetYear { get; set; }
        public float? Scope1Target { get; set; }
        public float? Scope2Target { get; set; }
        public float? Scope3Target { get; set; }
        public Guid OrganizationUnitId { get; set; }
        public Guid OrganizationTargetId { get; set; }
    }
}
