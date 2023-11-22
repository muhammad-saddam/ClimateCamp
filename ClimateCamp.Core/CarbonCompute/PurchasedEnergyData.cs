using ClimateCamp.Core.CarbonCompute;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// 
    /// </summary>
    [Table("PurchasedEnergyData", Schema = "Transactions")]
    public class PurchasedEnergyData : ActivityData
    {
        public GHG.EnergyType EnergyType { get; set; }
        public string Provider { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ContractualInstrument ContractualInstrument { get; set; }

        public PurchasedEnergyData()
        {
            //EmissionsSource = ; //set to the ID of the object that containes Purchased Energy as Name and Scope 2
            //UnitId = ;//kWh
            Name = "PurchasedEnergy";
            Description = "Utility Bill";
        }
        /// <summary>
        /// Distribution in percentages of purchased energy mix: Unknown, RenewableEnergy, FossilFuels, NuclearEnergy, Cogeneration
        /// </summary>
        public string EnergyMix { get; set; }
    }
}
