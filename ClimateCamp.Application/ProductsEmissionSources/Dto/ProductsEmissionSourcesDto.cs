using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ClimateCamp.CarbonCompute.ProductsEmissionSources))]
    public class ProductsEmissionSourcesDto : EntityDto<Guid>
    {
        public int EmissionsSourceId { get; set; }
        public Guid ProductEmissionsId { get; set; }
        public int? Availability { get; set; }
        public float? tCO2e { get; set; }
        public int? Methodology { get; set; }
        public float? PrimaryDataShare { get; set; }
    }
}
