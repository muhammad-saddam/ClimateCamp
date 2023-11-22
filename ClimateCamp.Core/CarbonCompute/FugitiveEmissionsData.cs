using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    [Table("FugitiveEmissionsData", Schema = "Transactions")]
    public class FugitiveEmissionsData : ActivityData
    {
        [ForeignKey(nameof(GreenhouseGasId))]
        public virtual GreenhouseGas GreenhouseGas { get; set; }
        public Guid GreenhouseGasId { get; set; }
    }
}
