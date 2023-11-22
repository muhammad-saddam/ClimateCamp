using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(OrganizationTarget))]
    public class OrganizationTargetDto : EntityDto<Guid>
    {
        public int TSFType { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
