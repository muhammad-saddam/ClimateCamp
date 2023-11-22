using ClimateCamp.CarbonCompute;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core.CarbonCompute
{
    /// <summary>
    /// 
    /// </summary>
    [Table("UseOfSoldProductsData", Schema = "Transactions")]
    public class UseOfSoldProductsData : ActivityData
    {
        public float? FuelUsedPerUseOfProduct { get; set; }

        public int? FuelUnitId { get; set; }

        public float? ElectricityConsumptionPerUseOfProduct { get; set; }

        public int? ElectricityUnitId { get; set; }

        public float? RefrigerantLeakagePerUseOfProduct { get; set; }

        public int? RefrigerantLeakageUnitId { get; set; }

    }
}
