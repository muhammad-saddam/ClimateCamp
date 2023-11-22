using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ProductEmissions))]
    public class ProductEmissionsDto : EntityDto<Guid>
    {
        public float? CO2eq { get; set; }
        public int? CO2eqUnitId { get; set; }
        public float? PrimaryDataShare { get; set; }
        public int? InventoryType { get; set; }
        public bool? Audited { get; set; }
        public string Certificate { get; set; }
        public string Auditor { get; set; }
        public int? Year { get; set; }
        public int? Period { get; set; }
        public int? PeriodType { get; set; }
        public Guid ProductId { get; set; }
        public Guid? OrganizationUnitId { get; set; }
        public int? EmissionSourceType { get; set; }
        public Guid? CustomerOrganizationId { get; set; }
        public bool? IsSelected { get; set; }
        public ICollection<ProductsEmissionSourcesDto> ProductsEmissionSources { get; set; }
        #region PACT ProductFootprint
        public string SpecVersion { get; set; }
        public int? Version { get; set; }
        public bool? IsActive { get; set; }
        public string StatusComment { get; set; }
        public string ProductFootprintComment { get; set; }
        public Guid? CarbonFootprintId { get; set; }


        #region Assurance (Audit). PACT section 4.4 https://wbcsd.github.io/data-exchange-protocol/v2/#elementdef-assurance
        public DateTime? AuditDate { get; set; }
        public DateTime? AuditUpdateDate { get; set; }
        public string AuditCoverage { get; set; }
        public string AuditLevel { get; set; }
        public string AuditStandardName { get; set; }
        public string AuditComments { get; set; }
        #endregion
        public DateTime? ValidityPeriodStart { get; set; }
        public DateTime? ValidityPeriodEnd { get; set; }
        #endregion
    }
}
