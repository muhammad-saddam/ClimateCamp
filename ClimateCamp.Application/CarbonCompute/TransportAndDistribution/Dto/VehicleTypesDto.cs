using ClimateCamp.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class VehicleTypesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Road, Rail, Air, etc <br/>
        /// <see cref="GHG.ModeOfTransport"/>
        /// </summary>
        public int ModeOfTransport { get; set; }
    }
}
