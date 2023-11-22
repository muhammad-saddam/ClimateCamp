
using ClimateCamp.CarbonCompute;
using ClimateCamp.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClimateCamp.EntityFrameworkCore.Seed.Host
{
    public class DefaultEmissionsFactorsCreator
    {
        private readonly CommonDbContext _context;
        public List<EmissionsFactor> InitialEmissionsFactor => GetInitialEmissionsFactors();
        public DefaultEmissionsFactorsCreator(CommonDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            CreateEmissionsFactors();
        }

        private void CreateEmissionsFactors()
        {
            foreach (var emissionfactorLibrary in InitialEmissionsFactor)
            {
                AddEmissionsFactorIfNotExists(emissionfactorLibrary);
            }
        }

        private void AddEmissionsFactorIfNotExists(EmissionsFactor emissionFactors)
        {
            if (_context.EmissionsFactors.IgnoreQueryFilters().Any(t => t.Library == emissionFactors.Library && t.EmissionsSource == emissionFactors.EmissionsSource))
            {
                return;
            }

            _context.EmissionsFactors.Add(emissionFactors);
            _context.SaveChanges();
        }

        private List<EmissionsFactor> GetInitialEmissionsFactors()
        {
            EmissionsFactorsLibrary library = _context.EmissionsFactorsLibrary.IgnoreQueryFilters().Where(t => t.Id == ClimateCampConsts.DefaultEmissionsFactorsLibraryId).FirstOrDefault();
            EmissionsSource source1 = _context.EmissionsSources.IgnoreQueryFilters().Where(t => t.Name == "Mobile Combustion").FirstOrDefault();

            Unit unitKG = _context.Units.IgnoreQueryFilters().Where(t => t.Name.ToLower() == "kg").FirstOrDefault();
            Unit unitTonnes = _context.Units.IgnoreQueryFilters().Where(t => t.Name == "t").FirstOrDefault();

            return new List<EmissionsFactor>
            {
                //scope1 emission factors Mobile Combustion
                new EmissionsFactor(library,source1, "","",true, "", 0.17363f,unitKG,0.00036f,unitKG, 0.00032f, unitKG, unitTonnes ),
            };

            //r(EmissionsFactorsLibrary library, EmissionsSource emissionsSource ,string name, 
            //string description,bool isActive, string documentationReference, OrganizationUnit organizationUnit,
            //float cO2, Unit cO2Unit, float n2O, Unit n2OUnit, float cH4, Unit cH4Unit)
        }
    }
}
