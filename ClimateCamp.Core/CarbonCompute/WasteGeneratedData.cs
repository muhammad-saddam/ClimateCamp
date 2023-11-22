using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Table for Waste Generated In Operations activity data
    /// </summary>
    [Table("WasteGeneratedData", Schema = "Transactions")]
    public class WasteGeneratedData : ActivityData
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
