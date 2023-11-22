using System;

namespace ClimateCamp.Application
{
    public class TransportationDataVM
    {
        public Guid Id { get; set; }
        public float? CO2e { get; set; }
        public int? CO2eUnitId { get; set; }
        public int DataQualityType { get; set; }
        public float Quantity { get; set; }
        public int? QuantityUnitId { get; set; }
        public string OrganizationUnitName { get; set; }
        public string Name { get; set; }
        public DateTime? ConsumptionStart { get; set; }
        public DateTime? ConsumptionEnd { get; set; }
        public string SupplierOrganization { get; set; }

    }
}
