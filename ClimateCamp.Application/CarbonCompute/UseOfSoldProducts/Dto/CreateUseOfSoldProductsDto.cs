using Abp.AutoMapper;
using ClimateCamp.Core.CarbonCompute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    public class CreateUseOfSoldProductsDto : ActivityDataDto
    {
        public float? FuelUsedPerUseOfProduct { get; set; }

        public int? FuelUnitId { get; set; }

        public float? ElectricityConsumptionPerUseOfProduct { get; set; }

        public int? ElectricityUnitId { get; set; }

        public float? RefrigerantLeakagePerUseOfProduct { get; set; }

        public int? RefrigerantLeakageUnitId { get; set; }

    }
}
