using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core.Features;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Edition.Dto
{
    [AutoMapFrom(typeof(Core.Editions.CustomEdition))]
    public class EditionDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatedUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifiedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string Discriminator { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsContactSales { get; set; }
        public string PriceLabel { get; set; }
        public List<EditionFeatureSettingCustom> Features { get; set; }
    }
}
