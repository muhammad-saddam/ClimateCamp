using Abp.Domain.Entities.Auditing;

using ClimateCamp.Core;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Storing calculation methods per organization or tenant.
    /// TODO: just a draft of an entity, incomplete
    /// </summary>
    [Table("CalculationModels", Schema = "Calculation")]
    public class CalculationModel : AuditedEntity<Guid>
    {
        public Guid? OrganizationUnitId { get; set; }
        public int? EmissionsSourceId { get; set; }


        public OrganizationUnit OrganizationUnit { get; set; }
        public virtual EmissionsSource EmissionsSource { get; set; }

        //public Guid EmissionsFactorsLibraryId { get; set; }
        //public virtual EmissionsFactorsLibrary EmissionsFactorsLibrary { get; set; }

        //TODO: add Property to represent the default or supported Unit.

        /// <summary>
        /// A value to represent the Function that invokes this logic
        /// </summary>
        public string CalculationFunction { get; set; }
    }
}
