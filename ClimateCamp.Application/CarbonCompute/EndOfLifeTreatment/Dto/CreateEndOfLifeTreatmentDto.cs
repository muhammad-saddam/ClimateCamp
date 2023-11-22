using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store the End-of-life treatment of sold products Activity Data
    /// </summary>
    [AutoMapTo(typeof(EndOfLifeTreatmentData))]
    public class CreateEndOfLifeTreatmentDto : ActivityDataDto
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
