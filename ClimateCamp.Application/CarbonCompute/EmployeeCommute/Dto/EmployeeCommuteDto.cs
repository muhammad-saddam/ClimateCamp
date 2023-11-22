using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(EmployeeCommuteData))]
    public class EmployeeCommuteDataDto : ActivityDataDto
    {
        public Guid VehicleTypeId { get; set; }
    }
}