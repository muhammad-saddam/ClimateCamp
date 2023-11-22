using CsvHelper.Configuration;
using FileHelpers;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    [DelimitedRecord(",")]
    public class PurchasedElectricityCSVUploadModel
    {
        /// <summary>
        /// Name of the Organization Unit
        /// </summary>
        public string OrganizationUnit { get; set; }
        /// <summary>
        /// Name of the Activity Type; ex: Purchased Electricity - Location Based
        /// </summary>
        public string ActivityDataType { get; set; }

        /// <summary>
        /// EAN Meter Number
        /// </summary>
        public string MeterNumber { get; set; }
        /// <summary>
        /// Exact Quantity of data 
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Measurement unit name; ex: kWh
        /// </summary>
        public string QuantityUnit { get; set; }
        /// <summary>
        /// Starting date of the activity
        /// </summary>
        public string ConsumptionStart { get; set; }
        /// <summary>
        /// End date of the activity
        /// </summary>
        public string ConsumptionEnd { get; set; }
        /// <summary>
        /// Billing date
        /// </summary>
        public string TransactionDate { get; set; }
        /// <summary>
        /// Energy Mix distribution in percentages:  Unknown, RenewableEnergy, FossilFuels, NuclearEnergy, Cogeneration
        /// </summary>
        public string EnergyMix { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PurchasedElectricityCSVUploadModelMap : ClassMap<PurchasedElectricityCSVUploadModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public PurchasedElectricityCSVUploadModelMap()
        {
            Map(m => m.OrganizationUnit).Name("organizationUnit");
            Map(m => m.ActivityDataType).Name("activityDataType");
            Map(m => m.MeterNumber).Name("meterNumber");
            Map(m => m.Quantity).Name("quantity");
            Map(m => m.QuantityUnit).Name("quantityUnit");
            Map(m => m.ConsumptionStart).Name("consumptionStart");
            Map(m => m.ConsumptionEnd).Name("consumptionEnd");
            Map(m => m.TransactionDate).Name("transactionDate");
            Map(m => m.EnergyMix).Name("energyMix");
        }
    }

}
