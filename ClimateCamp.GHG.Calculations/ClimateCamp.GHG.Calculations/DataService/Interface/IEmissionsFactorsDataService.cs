using ClimateCamp.CarbonCompute;
using System;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.DataService
{
    public interface IEmissionsFactorsDataService
    {
        Task<EmissionsFactor> GetEmisionFactors(int emissionsSourceId);

        /// <summary>
        /// This returns the EmissionsFactorsLibraryId based on the OrganizationUnitId <br/>
        /// The emissions factors used to calculate might differ from organization to organization <br/>
        /// </summary>
        /// <returns>Guid</returns>
        Task<Guid?> GetEmissionsFactorsLibraryId(Guid organizationUnitId);
        /// <summary>
        /// This return emission factors based on emission source and unit being provided
        /// </summary>
        /// <param name="emissionsSourceId"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        Task<EmissionsFactor> GetEmissionFactorsByEmissionSourceUnitId(int emissionsSourceId, int unitId, string libraryId);
    }
}
