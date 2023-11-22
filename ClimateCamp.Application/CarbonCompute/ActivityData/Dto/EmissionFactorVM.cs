using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application.CarbonCompute
{
    public class EmissionFactorVM
    {
        public Guid Id { get; set; }
        public int? EmissionSourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float? CO2e { get; set; }
        public int? CO2eUnitId { get; set; }
        public float? CO2 { get; set; }
        public int? CO2UnitId { get; set; }
        public int? UnitId { get; set; }
        public EmissionFactorLibrary EmissionFactorLibrary { get; set; }
        public Unit Unit { get; set; }
    }

    public class EmissionFactorLibrary {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }
}
