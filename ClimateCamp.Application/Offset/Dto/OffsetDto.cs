using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ClimateCamp.Core.Offset))]
    public class OffsetDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string DetailsTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public int PriceUnitId { get; set; }
        public string MaximumVolume { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
