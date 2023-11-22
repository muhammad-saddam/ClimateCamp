using Abp.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Energy, Transport
    /// Proposed values for Categories and Sectors from https://github.com/climatiq/Open-Emission-Factors-DB/blob/main/ID_STRUCTURE_GUIDANCE.md
    /// </summary>
    [Table("Sector", Schema = "Reference")]
    public class Sector : Entity<int>
    {

        public string Name { get; set; }

        public string Category { get; set; }
        public string Description { get; set; }
    }
}
