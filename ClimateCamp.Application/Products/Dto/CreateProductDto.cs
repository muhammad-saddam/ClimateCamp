using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(Product))]
    public class CreateProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
        public int Accuracy { get; set; }
        public int Status { get; set; }
        public int? UnitId { get; set; }
        public Guid? OrganizationId { get; set; }
        public string ImagePath { get; set; }
        public float? ProductionQuantity { get; set; }
        public string Description { get; set; }
        public Guid? ParentProductId { get; set; }
        public ICollection<ProductDto> Products { get; set; }
        public ICollection<ProductEmissionsDto> ProductEmissions { get; set; }
        public string CpcCode { get; set; }
    }
}
