using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{

    /// <summary>
    /// Table that stores transport and distribution activity data 
    /// </summary>
    [Table("TransportAndDistributionData", Schema = "Transactions")]
    public class TransportAndDistributionData : ActivityData
    {
        [ForeignKey(nameof(VehicleTypeId))]
        public virtual VehicleType VehicleType { get; set; }
        public Guid VehicleTypeId { get; set; }
        public string SupplierOrganization { get; set; }
        /// <summary>
        /// Represents either Upstream or Downstream. <br/>
        /// <see cref="Core.CarbonCompute.Enum.TransportAndDistributionType"/>
        /// </summary>
        public int Type { get; set; }
        public float? GoodsQuantity { get; set; }
        public int? GoodsUnitId { get; set; }
        public float? Distance  { get; set; }
        public int? DistanceUnitId { get; set; }

    }
}
