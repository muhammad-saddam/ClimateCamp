using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(CustomerProduct))]
    public class CreateCustomerProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        /// <summary>
        /// (Customer) company specific product identifier
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Customer Organization Id to which the CustomerProduct container belongs
        /// </summary>
        public Guid? OrganizationId { get; set; }
        /// <summary>
        ///Customer Product specific unit id
        /// </summary>
        public int? UnitId { get; set; }
        /// <summary>
        /// customer specific product  image
        /// </summary>
        public string ImagePath { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Product to which the CustomerProduct refers
        /// </summary>
        public Guid? ProductId { get; set; }
    }
}
