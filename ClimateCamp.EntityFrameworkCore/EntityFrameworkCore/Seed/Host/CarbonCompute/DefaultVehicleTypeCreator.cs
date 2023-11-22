
using Abp.MultiTenancy;
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultVehicleTypeCreator
    {
        private readonly CommonDbContext _context;
        public static List<VehicleType> InitialVehicleTypes => GetInitialVehicleTypes();
        public DefaultVehicleTypeCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateVehicleTypes();
        }

        private void CreateVehicleTypes()
        {
            foreach (var type in InitialVehicleTypes)
            {
                AddVehicleTypesIfNotExists(type);
            }
        }

        private void AddVehicleTypesIfNotExists(VehicleType type)
        {
            if (_context.VehicleTypes.IgnoreQueryFilters().Any(t => t.Name == type.Name))
            {
                return;
            }

            _context.VehicleTypes.Add(type);
            _context.SaveChanges();
        }

        private static List<VehicleType> GetInitialVehicleTypes()
        {
            var tenantId = ClimateCampConsts.MultiTenancyEnabled ? null : (int?)MultiTenancyConsts.DefaultTenantId;
            return new List<VehicleType>
            {
                new VehicleType("Gasoline Passenger Cars", "", 4, 1),
                new VehicleType("Gasoline Passenger Cars 1980-1990", "", 4, 1),
                new VehicleType("Passenger Cars-Diesel 2007-2021", "", 4, 1),
                new VehicleType("Passenger Cars-Diesel 1996-2006", "", 4, 1),
                new VehicleType("Passenger Cars-Diesel 1983-1995", "", 4, 1),
                new VehicleType("Passenger Cars-Diesel 1960-1982", "", 4, 1),
                new VehicleType("Gasoline Passenger Cars 1991-2000", "", 4, 1),
                new VehicleType("Gasoline Passenger Cars 2001-2010", "", 4, 1),
                new VehicleType("Gasoline Passenger Cars 2011-2021", "", 4, 1),
                new VehicleType("Bus", "", 4, 1),
                new VehicleType("Passenger Train", "", 2, 1),
                new VehicleType("Metro", "", 2, 1),
                new VehicleType("Passenger Airplane", "", 1, 1),
                new VehicleType("Bicycle", "", 4, 1)
            };
        }
    }
}
