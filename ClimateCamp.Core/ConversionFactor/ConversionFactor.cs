using Abp.Domain.Entities;
using ClimateCamp.CarbonCompute;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.Core
{
    /// <summary>
    /// Storing conversion factors for product
    /// </summary>
    [Table("ConversionFactors", Schema = "Master")]
    public class ConversionFactors : Entity<Guid>
    {
        [Required]
        public float ConversionFactor { get; set; }
        /// <summary>
        /// Represents the selected unit for the conversion factor. <br/>
        /// Changed from 'ProductUnit' to 'ConversionUnit'
        /// </summary>
        [Required]
        public int ConversionUnit { get; set; }
        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public Guid? ActivityDataId { get; set; }
        [ForeignKey(nameof(ActivityDataId))]
        public ActivityData ActivityData { get; set; }
        public bool IsActive { get; set; }

    }
}
