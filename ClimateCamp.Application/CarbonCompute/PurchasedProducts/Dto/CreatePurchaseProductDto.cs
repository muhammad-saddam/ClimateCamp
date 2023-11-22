using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(PurchasedProductsData))]
    public class CreatePurchaseProductDto : ActivityDataDto
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public int emissionSourceId { get; set; }
        [NotMapped]
        public Guid? EmissionGroupId { get; set; }
        public string BuyerAssignedSupplierId { get; set; }
    }
}
