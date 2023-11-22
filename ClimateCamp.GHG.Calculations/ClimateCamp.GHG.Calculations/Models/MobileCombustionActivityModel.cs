using System;

namespace Mobile.Combustion.Calculation.Models
{
    public class MobileCombustionActivityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ActivityTypeId { get; set; }
        public DateTime ConsumptionStart { get; set; }
        public DateTime ConsumptionEnd { get; set; }
        public DateTime TransactionDate { get; set; }
        public float Quantity { get; set; }
        public int UnitId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
