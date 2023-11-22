using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ConversionFactors))]
    public class ConversionFactorDto : EntityDto<Guid>
    {
        public float ConversionFactor { get; set; }
        public int ConversionUnit { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ActivityDataId { get; set; }
        public bool IsActive { get; set; }
    }
}
