using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(Offset))]
    public class CreateOffsetDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public string DetailsTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public int PriceUnitId { get; set; }
        public double MaximumVolume { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public string Link { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
