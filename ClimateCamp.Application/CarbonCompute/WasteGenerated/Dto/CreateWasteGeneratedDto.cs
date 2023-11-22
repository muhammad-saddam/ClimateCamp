using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store the Waste Generated In Operations Activity Data
    /// </summary>
    [AutoMapTo(typeof(WasteGeneratedData))]
    public class CreateWasteGeneratedDto : ActivityDataDto
    {
        /// <summary>
        /// See '<see cref="Core.CarbonCompute.Enum.WasteTreatmentMethod"/>' for the list of waste treatment methods.
        /// </summary>
        public int WasteTreatmentMethod { get; set; }
    }
}
