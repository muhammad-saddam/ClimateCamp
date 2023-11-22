using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// https://climatecamp.sharepoint.com/sites/ProductDev/Shared%20Documents/General/08.%20Ecosystem/Microsoft/All%20Stationary%20Combustions%204-2-2022%2012-30-25%20AM.xlsx?web=1
    /// By default assumes that the Quantity field represents the Volume or Weight.
    /// </summary>
    [Table("StationaryCombustionData", Schema = "Transactions")]
    public class StationaryCombustionData : ActivityData
    {

        /// <summary>
        /// Motor Gasoline, Diesel Fuel, Ethanol(100%)
        /// </summary>
        public virtual FuelType FuelType { get; set; }
        public Guid? FuelTypeId { get; set; }

        public string IndustrialProcessType { get; set; }

        public StationaryCombustionData()
        {
            //EmissionsSource = ; //set to the ID of the object that contains Mobile Combustion as Name and Scope 1

        }
    }

}
