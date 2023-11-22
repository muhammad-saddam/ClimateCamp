using Abp.Domain.Entities;
using ClimateCamp.Core.CarbonCompute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Buildings, Fleet vehicles or employee owned used for work, Activities (commute, travel etc)
    /// Mobile Combustion (Scope 1)
    /// Employee Commuting, BusinessTravel (Scope 3)
    /// </summary>
    [Table("EmissionsSources", Schema = "Reference")]
    public class EmissionsSource : Entity, IPassivable
    {
        public EmissionsSource()
        {

        }

        public EmissionsSource(GHG.EmissionScope emissionScope, string name, string description)
        {
            this.EmissionScope = emissionScope;
            this.Name = name;
            this.Description = description;
        }

        public GHG.EmissionScope EmissionScope { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Obsolete("The property is to be removed from the model, currently unused")]
        public virtual EmissionsFactor EmissionsFactors { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductsEmissionSources> ProductsEmissionSources { get; set; }
    }
}
