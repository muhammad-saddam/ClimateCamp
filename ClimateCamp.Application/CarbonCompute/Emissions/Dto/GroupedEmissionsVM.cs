using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    public class GroupedEmissionsVM
    {
        public Guid EmissionGroupId { get; set; }
        public string Icon { get; set; }
        public string Label { get; set; }
        public Guid? ParentEmissionGroupId { get; set; }
        public int? EmissionSourceId { get; set; }
        public string EmissionSourceName { get; set; }
        public Guid? ActivityDataId { get; set; }
        public float? CO2e { get; set; }
        public int? CO2eUnitId { get; set; }
        public float? Quantity { get; set; }
        public int? QuantityUnitId { get; set; }
        public DateTime? ConsumptionStart { get; set; }
        public DateTime? ConsumptionEnd { get; set; }
        public double? TotalEmissions { get; set; }
        public string ProductionQuantityUnit { get; set; }
        public ICollection<GroupedEmissionsVM> Children { get; set; }

    }
}
