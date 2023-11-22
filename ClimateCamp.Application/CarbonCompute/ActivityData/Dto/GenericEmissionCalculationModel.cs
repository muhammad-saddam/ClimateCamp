using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericEmissionCalculationRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid EmissionFactorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float UserConversionFactor { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GenericEmissionCalculationResponseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public float Emission{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsConversionFactorExist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float ConversionFactor { get; set; }
    }
}
