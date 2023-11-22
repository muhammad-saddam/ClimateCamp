using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.CarbonCompute
{

    /// <summary>
    /// m, km, kWh, USD, EUR, L
    /// Domain entity to represent the full inventory of all possible units of measurements.
    /// These range from standard length, weight, volume, money to more specific domain specific units like Data, PassengerOverDistance, WeightDistance and others.
    /// </summary>
    [Table("Units", Schema = "Reference")]
    public class Unit : Entity
    {

        public Unit()
        {

        }
        public Unit(int group, string name, bool isBaseUnit, float multiplier)
        {
            this.Group = group;
            this.Name = name;
            this.IsBaseUnit = isBaseUnit;
            this.Multiplier = multiplier;
        }

        public int Group { get; set; }
        public string Name { get; set; }
        public bool IsBaseUnit { get; set; } = true;
        public float Multiplier { get; set; } = 1;

        
    }
}
