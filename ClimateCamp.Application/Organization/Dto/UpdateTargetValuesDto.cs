using Abp.Application.Services.Dto;
using System;

namespace ClimateCamp.Application
{
    public class UpdateTargetValuesDto : EntityDto<Guid>
    {
        public int BaseLineYear { get; set; }
        public decimal BaseLineEmission { get; set; }
        public decimal Target { get; set; }
    }
}
