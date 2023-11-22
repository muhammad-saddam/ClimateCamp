using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ClimateCamp.Feature.Dto
{
    [AutoMapFrom(typeof(Core.Features.EditionFeatureSettingCustom))]
    public class FeatureDto : EntityDto<long>
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Discriminator { get; set; }
        public int? EditionId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public string Icon { get; set; }
        public bool? IsActive { get; set; }
        public long ParentId { get; set; }
        public bool? ShowActiveLabel { get; set; }
    }
}
