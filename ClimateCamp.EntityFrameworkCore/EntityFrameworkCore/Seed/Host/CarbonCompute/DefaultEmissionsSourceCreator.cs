using ClimateCamp.CarbonCompute;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultEmissionsSourceCreator
    {
        private readonly CommonDbContext _context;
        public static List<EmissionsSource> InitialEmissionsSources => GetInitialEmissionsSources();
        public DefaultEmissionsSourceCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateEmissionsSources();
        }

        private void CreateEmissionsSources()
        {
            foreach (var emissionSource in InitialEmissionsSources)
            {
                AddEmissionsSourceIfNotExists(emissionSource);
            }
        }

        private void AddEmissionsSourceIfNotExists(EmissionsSource emissionSource)
        {
            if (_context.EmissionsSources.IgnoreQueryFilters().Any(t => t.Name == emissionSource.Name && t.EmissionScope == emissionSource.EmissionScope))
            {
                return;
            }

            _context.EmissionsSources.Add(emissionSource);
            _context.SaveChanges();
        }

        private static List<EmissionsSource> GetInitialEmissionsSources()
        {
            //TODO: for Vitalit to temporarily set ON. Cannot insert explicit value for identity column in table 'EmissionsSources' when IDENTITY_INSERT is set to OFF.

            return new List<EmissionsSource>
            {
                new EmissionsSource { Id = 1, Name = "Mobile Combustion", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope1, IsActive = true },
                new EmissionsSource { Id = 4, Name = "Purchased Electricity", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope1, IsActive = true },
                new EmissionsSource { Id = 5, Name = "Stationary Combustion", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope2, IsActive = true },
                new EmissionsSource { Id = 10, Name = "Purchased goods & services", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope3, IsActive = true },
                new EmissionsSource { Id = 11, Name = "Upstream transportation", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope3, IsActive = false },
                new EmissionsSource { Id = 12, Name = "Downstream transportation", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope3, IsActive = false },
                new EmissionsSource { Id = 13, Name = "Use of sold products", Description = string.Empty, EmissionScope = GHG.EmissionScope.Scope3, IsActive = false },

            };
        }
    }
}
