using Abp.Application.Services.Dto;
using ClimateCamp.Common.Users.Dto;
using System;

namespace ClimateCamp.Application
{
    public class ProductRequestManagementDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }

        /// <summary>
        /// supplier product request (outgoing request): organization id of the supplier organization
        /// customer product request (incoming request): organization id of the customer organization
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// CO2eq in kg per product unit
        /// </summary>
        public float? CO2eq { get; set; }
        
        public int? CO2eqUnitId { get; set; }

        public int Accuracy { get; set; }
        public int Status { get; set; }   

        public int UnitId { get; set; }
        public string ProductUnit { get; set; }
        public string EmissionUnit { get; set; }

        public UserDto CreatorUser { get; set; }
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public string SupplierOrganization { get; set; }
        public string SupplierOrganizationImage { get; set; }
        public float? ProductionQuantity { get; set; }

        public bool IsUsedStatus { get; set; }

        //collaboration incoming overview table
        public string ProductCorrelated { get; set; }
        public string ProductRequested { get; set; }
    }
}
