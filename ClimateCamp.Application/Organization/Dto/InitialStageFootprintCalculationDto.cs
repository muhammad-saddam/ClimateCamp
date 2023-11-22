using Abp.Application.Services.Dto;
using System;

namespace ClimateCamp.Application
{
#pragma warning disable CS1591
    public class InitialStageFootprintCalculationDto : EntityDto<Guid>
    {
        public decimal BaseLineEmission { get; set; }
    }
}