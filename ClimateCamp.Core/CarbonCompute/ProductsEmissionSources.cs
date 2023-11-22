using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Bridge table to store Product emission sources
    /// </summary>
    [Table("ProductsEmissionSources", Schema = "Reference")]
    public class ProductsEmissionSources : Entity<Guid>
    {
        public int EmissionsSourceId { get; set; }
        public virtual EmissionsSource EmissionsSource { get; set; }
        public Guid? ProductEmissionsId { get; set; }
        public virtual ProductEmissions ProductEmissions { get; set; }

        public int? Availability { get; set; }
        public float? tCO2e { get; set; }
        public int? Methodology { get; set; }
        public float? PrimaryDataShare { get; set; }

    }
}
