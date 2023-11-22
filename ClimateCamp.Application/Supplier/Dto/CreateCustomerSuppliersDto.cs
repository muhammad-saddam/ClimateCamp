using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMapTo(typeof(CustomerSupplier))]

    public class CreateCustomerSuppliersDto : EntityDto<long>
    {
        /// <summary>
        /// Name of the organisation
        /// </summary>
        [Required]
        public string Name { get; set; }

        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }

        [Required]
        public string ContactEmailAddress { get; set; }

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
        public string Phone { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class CreateSupplierInput : CreateCustomerSuppliersDto, IEntityDto<long>
    {
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid CustomerOrganizationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid SupplierOrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Id of User who invites the supplier & creates the supplier organization
        /// </summary>
        public long? CreatorUserId { get; set; }
    }

    public class GetSupplierOutput : CreateSupplierInput
    {
        public OrganizationDto SupplierOrganization { get; set; }
    }

    public class GetAllSuppliersOutput
    {

    }
}
