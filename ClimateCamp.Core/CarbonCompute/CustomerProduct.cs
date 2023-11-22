using Abp.Domain.Entities.Auditing;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{

    /// <summary>
    /// Entity used to store customer product as a seperate product for seggregation purposes
    /// to keep track of requested and correlated products in collaboration section
    /// </summary>
    [Table("CustomerProducts", Schema = "Master")]
    public class CustomerProduct : FullAuditedEntity<Guid, User>
    {
        /// <summary>
        /// Customer specific name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// (Customer) company specific product identifier
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Customer Organization Id to which the CustomerProduct container belongs
        /// </summary>
        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }
        /// <summary>
        /// Customer Product specific unit id
        /// </summary>
        public int? UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit Unit { get; set; }
        /// <summary>
        /// customer specific product  image
        /// </summary>
        public string ImagePath { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Product to which the CustomerProduct refers
        /// </summary>
        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
