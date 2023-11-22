using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;

namespace ClimateCamp.Application
{

    [AutoMapTo(typeof(Unit))]
    public class CreateUnitDto : EntityDto<int>
    {
        public UnitGroup Group { get; set; }
        public string Name { get; set; }
        public bool IsBaseUnit { get; set; }
        public float Multiplier { get; set; }

    }

}
