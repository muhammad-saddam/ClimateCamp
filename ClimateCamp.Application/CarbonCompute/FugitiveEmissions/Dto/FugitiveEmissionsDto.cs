using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(FugitiveEmissionsData))]
    public class FugitiveEmissionsDataDto : ActivityDataDto
    {
        public Guid GreenhouseGasId { get; set; }
    }
}
