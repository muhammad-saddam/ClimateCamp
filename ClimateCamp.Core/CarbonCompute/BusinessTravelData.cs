using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Table for business travel activity data 
    /// </summary>
    [Table("BusinessTravelData", Schema = "Transactions")]
    public class BusinessTravelData : ActivityData
    {
        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType VehicleType { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
