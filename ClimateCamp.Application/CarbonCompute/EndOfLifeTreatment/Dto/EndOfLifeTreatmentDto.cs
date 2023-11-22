using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    /// <summary>
    ///  Dto to retrieve the End-of-life treatment of sold products Activity Data
    /// </summary>
    [AutoMapFrom(typeof(EndOfLifeTreatmentData))]
    public class EndOfLifeTreatmentDataDto : ActivityDataDto
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
