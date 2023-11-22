using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{

    [AutoMapTo(typeof(FuelAndEnergyData))]
    public class CreateFuelAndEnergyDto : ActivityDataDto
    {
        public Guid? FuelTypeId { get; set; }

        public GHG.EnergyType? EnergyType { get; set; }
    }
}