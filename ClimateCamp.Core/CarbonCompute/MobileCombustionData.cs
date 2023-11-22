using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// https://climatecamp.sharepoint.com/sites/ProductDev/Shared%20Documents/General/08.%20Ecosystem/Microsoft/All%20Mobile%20Combustions%2011-9-2021%2010-40-57%20PM.xlsx?web=1
    /// By default assumes that the Quantity field represents the Volume.
    /// </summary>
    [Table("MobileCombustionData", Schema = "Transactions")]
    public class MobileCombustionData : ActivityData
    {

        /// <summary>
        /// Motor Gasoline, Diesel Fuel, Ethanol(100%)
        /// </summary>
        public FuelType FuelType { get; set; }

        public VehicleType VehicleType { get; set; }

        public float? Distance { get; set; }
        public virtual Unit? DistanceUnit { get; set; }
        public int? DistanceUnitId { get; set; }
        public MobileCombustionData()
        {
            //EmissionsSource = ; //set to the ID of the object that contains Mobile Combustion as Name and Scope 1

        }
    }

}
