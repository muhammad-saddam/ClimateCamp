using Abp.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// life cycle assessment(LCA) activities.
    /// 
    ///  "electricity_generation",
    ///   "end_of_life",
    ///      "fuel_combustion",
    ///      "gate_to_grave",
    ///      "transmission_and_distribution",
    ///      "unknown",
    ///      "upstream",
    ///      "use_phase",
    ///      "well_to_tank"
    ///      fuel_upstream or manufacturing
    ///      use phase (e.g. fuel_combustion)
    ///      and end-of-life (e.g. end_of_life or gate_to_grave)
    /// </summary>
    [Table("LifeCycleActivity", Schema = "Reference")]
    public class LifeCycleActivity : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public LifeCycleActivity()
        {
            Name = "unknown";
        }
    }
}
