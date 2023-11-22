using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Supplier.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMapFrom(typeof(CustomerSupplier))]
    public class CustomerSuppliersDto : EntityDto<long>
    {
        /// <summary>
        /// Name of the organisation
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactPersonFirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactPersonLastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactEmailAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Tons of purchased products yearly
        /// </summary>
        public long? YearlyProductQuantity { get; set; }

        public string? Service { get; set; }

        /// <summary>
        /// Yearly invoiced services in €
        /// </summary>
        public long? YearlyServiceQuantity { get; set; }

        /// <summary>
        /// Country of origin
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string? PersonalMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid CustomerOrganizationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Scored, To Invite, Requested
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// CO3E total usage
        /// </summary>
        public float? CO2E { get; set; }
        public Guid SupplierOrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }

        public long? CreatorUserId { get; set; }
    }
}
