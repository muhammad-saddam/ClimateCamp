using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store the Fugitive Emissions Activity Data
    /// </summary>
    [AutoMapTo(typeof(FugitiveEmissionsData))]
    public class CreateFugitiveEmissionsDto : ActivityDataDto
    {
        public Guid GreenhouseGasId { get; set; }
    }
}
