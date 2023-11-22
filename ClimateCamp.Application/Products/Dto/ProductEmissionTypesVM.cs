using System;

namespace ClimateCamp.Application
{
    public class ProductEmissionTypesVM
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string OrganizationName { get; set; }
        public int Type { get; set; }
        public int? EmissionType { get; set; }
        public string EmissionTypeName { get; set; }
        public float? CO2eq { get; set; }
        public int? CO2eqUnitId { get; set; }
        public string EmissionFactorName { get; set; }
        public int? EmissionFactorYear { get; set; }
        public bool? IsSelected { get; set; }
        public int? Year { get; set; }
    }
}
