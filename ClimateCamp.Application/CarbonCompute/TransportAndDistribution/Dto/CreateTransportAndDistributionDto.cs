using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Dto to store Transport and Distribution data
    /// </summary>
    [AutoMapTo(typeof(TransportAndDistributionData))]
    public class CreateTransportAndDistributionDataDto : ActivityDataDto
    {
        public Guid VehicleTypeId { get; set; }
        public string SupplierOrganization { get; set; }
        /// <summary>
        /// Represents either Upstream or Downstream.<br/> 
        /// <see cref="Core.CarbonCompute.Enum.TransportAndDistributionType"/>
        /// </summary>
        public int Type { get; set; }
        public float? GoodsQuantity { get; set; }
        public int? GoodsUnitId { get; set; }
        public float? Distance { get; set; }
        public int? DistanceUnitId { get; set; }
    }
}
