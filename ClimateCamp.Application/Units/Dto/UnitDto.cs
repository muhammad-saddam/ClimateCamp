using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(Unit))]
    public class UnitDto : EntityDto<int>
    {
        public int Group { get; set; }
        public string Name { get; set; }
        public bool IsBaseUnit { get; set; }
        public float Multiplier { get; set; }
    }

}
