using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClimateCamp.CarbonCompute
{
    [Table("ActivityTypes", Schema = "Reference")]
    public class ActivityType : Entity, IMayHaveTenant
    {
        public ActivityType()
        {

        }
        public ActivityType(int? tenantId, EmissionsSource source, int id, string name)
        {
            TenantId = tenantId;
            EmissionsSource = source;
            EmissionsSourceId = id;
            Name = name;
        }

        public virtual int? TenantId { get; set; }

        /// <summary>
        /// ex. Mobile Combustion
        /// 
        /// Upstream transportation and distribution
        /// Business travel
        /// Employee commuting
        /// Downstream transportation and distribution
        /// </summary>
        public virtual EmissionsSource EmissionsSource { get; set; }

        public int EmissionsSourceId { get; set; }

        /// <summary>
        /// ex. Distance Activity, Fuel Usage Activity
        /// 
        /// Distance, Passenger Distance, Vehicle Distance, Weight Distance 
        /// </summary>
        public virtual string Name { get; set; }

    }
}
