using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Table for End-of-life treatment of sold products activity data
    /// </summary>
    [Table("EndOfLifeTreatmentData", Schema = "Transactions")]
    public class EndOfLifeTreatmentData : ActivityData
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
