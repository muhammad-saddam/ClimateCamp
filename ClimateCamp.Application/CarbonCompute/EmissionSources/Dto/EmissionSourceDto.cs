
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(EmissionsSource))]
    public class EmissionSourceDto : EntityDto<int>
    {
        public GHG.EmissionScope EmissionScope { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
