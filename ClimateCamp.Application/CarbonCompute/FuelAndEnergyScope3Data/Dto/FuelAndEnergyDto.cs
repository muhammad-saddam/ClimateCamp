using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(FuelAndEnergyData))]
    public class FuelAndEnergyDto : ActivityDataDto
    {
        public Guid? FuelTypeId { get; set; }

        public GHG.EnergyType? EnergyType { get; set; }
    }
}