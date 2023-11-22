using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store business travel data
    /// </summary>
    [AutoMapTo(typeof(BusinessTravelData))]
    public class CreateBusinessTravelDataDto : ActivityDataDto
    {
        public Guid VehicleTypeId { get; set; }
    }
}
