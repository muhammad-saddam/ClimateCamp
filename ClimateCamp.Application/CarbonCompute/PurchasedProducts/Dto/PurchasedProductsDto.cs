using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store Purchase Goods Data
    /// </summary>
    [AutoMapFrom(typeof(PurchasedProductsData))]
    public class PurchasedProductsDataDto : ActivityDataDto
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string BuyerAssignedSupplierId { get; set; }
    }
}
