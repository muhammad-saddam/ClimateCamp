using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store Employee Commute data
    /// </summary>
    [AutoMapTo(typeof(EmployeeCommuteData))]
    public class CreateEmployeeCommuteDataDto : ActivityDataDto
    {
        public Guid VehicleTypeId { get; set; }
    }
}
