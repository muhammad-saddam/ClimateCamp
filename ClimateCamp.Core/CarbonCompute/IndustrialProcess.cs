using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

using System.ComponentModel.DataAnnotations.Schema;


namespace ClimateCamp.CarbonCompute
{
    /// <summary>
    /// Mode to represent the emissions of an industrial processes, examples:
    /// Cement Production
    /// Chemical Process
    /// Coal and Coke
    /// Diesel Fuel
    /// HVAC
    /// Motor Gasoline
    /// Natural Gas
    /// </summary>
    [Table("IndustrialProcesses", Schema = "Transactions")]
    public class IndustrialProcess : AuditedEntity<Guid>, IMayHaveTenant
    {
        public string Name { get; set; }
        /// <summary>
        /// If null then shared processes for all organizations
        /// </summary>
        public int? TenantId { get; set; }
    }
}
