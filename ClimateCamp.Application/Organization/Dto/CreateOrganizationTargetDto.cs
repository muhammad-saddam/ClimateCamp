using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(OrganizationTarget))]
    public class CreateOrganizationTargetDto : EntityDto<Guid>
    {
        public int TSFType { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
