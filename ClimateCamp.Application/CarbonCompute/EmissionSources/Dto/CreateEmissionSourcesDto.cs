using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(EmissionsSource))]
    public class CreateEmissionSourcesDto : EntityDto
    {
        public GHG.EmissionScope EmissionScope { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
