using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(ProductGroups))]
    public class CreateProductGroupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? ParentProductGroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
