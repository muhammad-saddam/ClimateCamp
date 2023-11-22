using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ClimateCamp.Common.Authorization.Users;
using ClimateCamp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{

    /// <summary>
    /// TODO: Values to refer to the GHG Excel file and add new fields as needed
    /// </summary>
    [Table("Products", Schema = "Reference")]
    public class Product : FullAuditedEntity<Guid, User>, IPassivable
    {
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
        public int Accuracy { get; set; }
        public int Status { get; set; }
        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; }

        /// <summary>
        /// We use this one to get the unit of the product. For PACT, it can only be liter, kilogram, cubic meter, kilowatt hour, megajoule, ton kilometer or squre meter. <br/>
        /// See <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#enumdef-declaredunit">PACT DeclaredUnit</a> for more info.<br/>
        /// PACT property name: 'declaredUnit', <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public int? UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Unit Unit { get; set; }
        public string ImagePath { get; set; }
        public float? ProductionQuantity { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(ParentProductId))]
        public virtual Product ParentProduct { get; set; }
        public Guid? ParentProductId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public ICollection<ProductEmissions> ProductEmissions { get; set; }
        public ICollection<CustomerProduct> CustomerProducts { get; set; }

        /// <summary>
        /// A UN Product Classification Code (CPC) that the given product belongs to. <br/>
        /// PACT property name: 'productCategoryCpc', <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string CpcCode { get; set; }

    }
}
