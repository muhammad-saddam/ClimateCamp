using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{

    /// <summary>
    /// Table for store purchase goods and services emission source activity data
    /// </summary>
    [Table("PurchasedProductsData", Schema = "Transactions")]
    public class PurchasedProductsData : ActivityData
    {
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }

        /// <summary>
        /// Represents the supplier (vendor) id but from a buyers perspective (defined by the buyer). <br/>
        /// This supports the <a href="https://wbcsd.github.io/data-exchange-protocol/v2/#companyidset">PACT property: companyIds</a> which should be a non-empty array of URNs. <br/>
        /// PACT property name: 'companyIds', <br/>
        /// PACT Required: Mandatory
        /// </summary>
        public string BuyerAssignedSupplierId { get; set; }
        // <summary>
        /// CO2eq in kg per product unit
        /// </summary>
        //public float? CO2eq { get; set; }
        //public int? CO2eqUnitId { get; set; }
    }
}
