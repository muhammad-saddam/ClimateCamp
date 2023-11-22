using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// https://climatecamp.sharepoint.com/sites/ProductDev/Shared%20Documents/General/08.%20Ecosystem/Microsoft/Active%20Greenhouse%20Gases%2011-9-2021%2010-48-59%20PM.xlsx?web=1
    /// </summary>
    [Table("GreenhouseGases", Schema = "Reference")]
    public class GreenhouseGas : Entity<Guid>, IPassivable
    {
        public GreenhouseGas()
        {

        }

        public GreenhouseGas(string code, string name, GHG.GreenhouseGasCategory category, string desription, int gwpProtocol, bool isActive)
        {
            this.Code = code;
            this.Name = name;
            this.Category = category;
            this.Description = desription;
            this.GWPFactor = gwpProtocol;
            this.IsActive = isActive;
        }

        /// <summary>
        /// CO2, CH4, N20, SF6, NF3, HFC, PFC
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Carbon Dioxide, Methane, Nitrous oxide, Sulphur hexafluoride, Nitrogen triflouride, Hydrofluorocarbons, Perfluorocarbons
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// HFC, PFC, NF3, SF6, Non Fluorinated
        /// </summary>
        [Required]
        public GHG.GreenhouseGasCategory Category { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Global Warming Potential: GWP at 100 years from the 5th report of the IPCC
        /// 1, 28, 265, 26100, 16100
        /// </summary>
        [Required]
        public float GWPFactor { get; set; }

        public bool IsActive { get; set; }
        public string ArVersion { get; set; }
    }
}
