using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultEmissionsFactorsLibraryCreator
    {
        private readonly CommonDbContext _context;
        public static List<EmissionsFactorsLibrary> InitialEmissionsFactorsLibrary => GetInitialEmissionsFactorsLibraries();
        public DefaultEmissionsFactorsLibraryCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateEmissionsFactorsLibrarys();
        }

        private void CreateEmissionsFactorsLibrarys()
        {
            foreach (var emissionfactorLibrary in InitialEmissionsFactorsLibrary)
            {
                AddEmissionsFactorsLibraryIfNotExists(emissionfactorLibrary);
            }
        }

        private void AddEmissionsFactorsLibraryIfNotExists(EmissionsFactorsLibrary emissionFactorsLibrary)
        {
            if (_context.EmissionsFactorsLibrary.IgnoreQueryFilters().Any(t => t.Name == emissionFactorsLibrary.Name))
            {
                return;
            }

            _context.EmissionsFactorsLibrary.Add(emissionFactorsLibrary);
            _context.SaveChanges();
        }

        private static List<EmissionsFactorsLibrary> GetInitialEmissionsFactorsLibraries()
        {
            return new List<EmissionsFactorsLibrary>
            {
                new EmissionsFactorsLibrary(null, " UK Department for Environment Food & Rural Affairs", 1, true){ Id = ClimateCampConsts.DefaultEmissionsFactorsLibraryId }
            };
        }
    }
}
