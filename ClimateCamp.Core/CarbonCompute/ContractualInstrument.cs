using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.Core.CarbonCompute
{
    /// <summary>
    /// https://ghgprotocol.org/sites/default/files/Scope2_ExecSum_Final.pdf
    /// Contractual instruments: Any type of contract between two parties for the sale and purchase of energy bundled with attributes about the energy generation, or for unbundled attribute claims.
    /// Markets differ as to what contractual instruments are commonly available or used by companies to purchase energy or claim specific attributes about it, 
    /// but they can include energy attribute certificates(RECs, GOs, etc.), 
    /// direct contracts(for both low-carbon, renewable, or fossil fuel generation), 
    /// supplier-specific emission rates, 
    /// and other default emission factors representing the untracked or unclaimed energy and emissions(termed the residual mix) if a company does not have other contractual information that meets the Scope 2 Quality Criteria.
    /// 
    /// Energy attribute certificate: A category of contractual instrument that represents certain information (or attributes) about the energy generated, but does not represent the energy itself.
    /// This category includes a variety of instruments with different names, including certificates, tags, credits, or generator declarations.For the purpose of this guidance, the term “energy attribute certificates” or just “certificates” will be used as the general term for this category of instruments.
    /// 
    /// Energy generation facility: Any technology or device that generates energy for consumer use, including everything from utility-scale fossil fuel power plants to rooftop solar panels.
    /// 
    /// Energy supplier: Also known as an electric utility, this is the entity that sells energy to consumers and can provide information regarding the GHG intensity ofdelivered electricity.
    /// 
    /// Generators: Here used to mean the entity that owns or operates an energy generation facility. 
    /// 
    /// Green power product/green tariff: A consumer option offered by an energy supplier distinct from the “standard”  offering.These are often renewables or other low-carbon energy sources, supported by energy attribute certificates or other contracts
    /// 
    /// https://www.epa.gov/sites/default/files/2016-03/documents/electricityemissions_3_2016.pdf
    /// 
    /// 
    /// Location-Based Electricity Emission Factors
    /// 1. Direct Line Emission Factor
    /// 2. Regional Emission Factor
    /// 2. National Emission Factor
    /// 
    ///  Market-Based Electricity Emission Factors
    ///  1. Energy Attribute Certificates = renewable energy certificates (RECs) or Guarantees of Origin (GOs).
    ///  2. Contracts =  power purchase agreement (PPA) ), to purchase electricity from a specified generating facility, which may be located at the organization’s facility, at a nearby location with a direct line connection to the organization
    ///  3. Supplier-Specific Emission Factor
    ///  4. Residual Mix Factor (the preferred market-based default emission factors when the above are not available).
    ///  5. Regional Emission Factor 
    ///  6. National Emission Factor

    /// </summary>
    [Table("ContractualInstruments", Schema = "Transactions")]
    public class ContractualInstrument : Entity, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public EnergySource EnergySource { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// This can include certification such as Green-e Energy (U.S.), EcoLogo (Canada), or labels such as EKOenergy and Naturemade in the EU. 
        /// The certification or label name should also specify what is being certified, e.g. in the U.S.Green-e Energy certifies against a set of requirements described in their National Standard.
        /// </summary>
        public string Label { get; set; }
    }
}
