using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultGreenhouseGasesCreator
    {
        private readonly CommonDbContext _context;
        public static List<GreenhouseGas> InitialGHGs => GetInitialGHGs();
        public DefaultGreenhouseGasesCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateGHGs();
        }

        private void CreateGHGs()
        {
            foreach (var gas in InitialGHGs)
            {
                AddGHGIfNotExists(gas);
            }
        }

        private void AddGHGIfNotExists(GreenhouseGas gas)
        {
            if (_context.GreenHouseGases.IgnoreQueryFilters().Any(t => t.Name == gas.Name && t.IsActive == true))
            {
                return;
            }

            _context.GreenHouseGases.Add(gas);
            _context.SaveChanges();
        }

        private static List<GreenhouseGas> GetInitialGHGs()
        {
            return new List<GreenhouseGas>
            {
                new GreenhouseGas("CO2", "Carbon Dioxide",GHG.GreenhouseGasCategory.NonFluorinated, "",  1, true),
                new GreenhouseGas("CH4", "Methane",GHG.GreenhouseGasCategory.NonFluorinated, "",  25, true),
                new GreenhouseGas("N2O", "Nitrous oxide",GHG.GreenhouseGasCategory.NonFluorinated, "",  298, true)
            };
        }
    }
}
