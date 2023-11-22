using Abp.Domain.Entities;

using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// TODO: Values to refer to the GHG Excel file and add new fields as needed
    /// </summary>
    [Table("FuelTypes", Schema = "Reference")]
    public class FuelType : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
