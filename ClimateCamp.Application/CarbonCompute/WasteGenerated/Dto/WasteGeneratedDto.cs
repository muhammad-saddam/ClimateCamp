using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    /// <summary>
    ///  Dto to retrieve the Waste Generated In Operations Activity Data
    /// </summary>
    [AutoMapFrom(typeof(WasteGeneratedData))]
    public class WasteGeneratedDataDto : ActivityDataDto
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
