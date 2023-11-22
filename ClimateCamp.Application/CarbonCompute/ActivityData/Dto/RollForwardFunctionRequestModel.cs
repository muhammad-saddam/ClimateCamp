using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class RollForwardFunctionRequestModel
    {
        public Guid? OrganizationId { get; set; }
        public DateTime? ConsumptionStart { get; set; }
        public DateTime? ConsumptionEnd { get; set; }
        public DateTime? TargetPeriodStart { get; set; }
        public DateTime? TargetPeriodEnd { get; set; }

    }
}
