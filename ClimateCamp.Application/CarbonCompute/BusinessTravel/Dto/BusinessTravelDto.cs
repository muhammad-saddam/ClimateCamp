using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(BusinessTravelData))]
    public class BusinessTravelDataDto : ActivityDataDto
    {
        public Guid VehicleTypeId { get; set; }
    }
}