using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(ConversionFactors))]
    public class CreateConversionFactorDto : EntityDto<Guid>
    {
        public float ConversionFactor { get; set; }
        public int ConversionUnit { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ActivityDataId { get; set; }
        public bool? IsActive { get; set; }
    }
}
