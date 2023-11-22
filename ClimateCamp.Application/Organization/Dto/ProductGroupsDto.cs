using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ProductGroups))]
    public class ProductGroupsDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
        public Guid? ParentProductGroupId { get; set; }
        public ICollection<ProductGroupsDto> Children { get; set; }
    }
}
