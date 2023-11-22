using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static ClimateCamp.CarbonCompute.GHG;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultUnitCreator
    {
        private readonly CommonDbContext _context;
        public static List<Unit> InitialUnits => GetInitialUnits();
        public DefaultUnitCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateUnits();
        }

        private void CreateUnits()
        {
            foreach (var unit in InitialUnits)
            {
                AddUnitIfNotExists(unit);
            }
        }

        private void AddUnitIfNotExists(Unit unit)
        {
            if (_context.Units.IgnoreQueryFilters().Any(t => t.Name == unit.Name))
            {
                return;
            }

            _context.Units.Add(unit);
            _context.SaveChanges();
        }

        private static List<Unit> GetInitialUnits()
        {
            return new List<Unit>
            {
                new Unit((int)UnitGroup.Weight,"kg", true,  1),
                new Unit((int)UnitGroup.Weight,"t", false,  1000),
                new Unit((int)UnitGroup.Distance,"km", true,  1),
                new Unit((int)UnitGroup.Distance,"miles", false,  1.609f),
                new Unit((int)UnitGroup.Currency,"EUR", true,  1),
                new Unit((int)UnitGroup.Weight,"lb", true,  0.45359236f),
                new Unit((int)UnitGroup.Volume,"l", true,  1),
                new Unit((int)UnitGroup.Energy,"KWh", true,  1),
                new Unit((int)UnitGroup.Volume,"m3", false,  1000),
            };
        }
    }
}
