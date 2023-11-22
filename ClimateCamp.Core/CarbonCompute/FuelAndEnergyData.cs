using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{

    [Table("FuelAndEnergyData", Schema = "Transactions")]
    public class FuelAndEnergyData : ActivityData
    {

        public Guid? FuelTypeId { get; set; }

        public GHG.EnergyType? EnergyType { get; set; }

    }

}
