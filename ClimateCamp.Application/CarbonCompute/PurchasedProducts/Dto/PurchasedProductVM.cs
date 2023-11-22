using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    public class PurchasedProductVM
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? OrganizationUnitId { get; set; }
        public string Supplier { get; set; }
        public string OrgUnitName { get; set; }
        public string Name { get; set; }
        public int Accuracy { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }
        public DateTime ConsumptionStart { get; set; }
        public DateTime ConsumptionEnd { get; set; }
        public DateTime TransactionDate { get; set; }
        public float Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string ProductCode { get; set; }
        public float? CO2eq { get; set; }
        public int? CO2eqUnitId { get; set; }

    }
}
