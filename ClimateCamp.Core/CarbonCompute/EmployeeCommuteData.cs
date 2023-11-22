using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Table for employee commute activity data 
    /// </summary>
    [Table("EmployeeCommuteData", Schema = "Transactions")]
    public class EmployeeCommuteData : ActivityData
    {
        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType VehicleType { get; set; }
        public Guid VehicleTypeId { get; set; }
    }
}
