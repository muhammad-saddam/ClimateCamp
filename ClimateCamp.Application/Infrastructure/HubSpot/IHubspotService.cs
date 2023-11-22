using ClimateCamp.Infrastructure.Models;
using System.Threading.Tasks;

namespace ClimateCamp.Infrastructure.HubSpot
{
    /// <summary>
    /// Hubspot API methods
    /// </summary>
    public interface IHubspotService
    {
        /// <summary>
        /// Get hubspot company information by workemail
        /// </summary>
        /// <param name="workEmail"></param>
        /// <returns></returns>
        Task<HubSpotCompanySearchByDomainResponseModel> GetHubSpotCompanyByDomainAsync(string workEmail);
        /// <summary>
        /// Create Hubspot company 
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>
        Task<long> CreateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel);

        /// <summary>
        /// Update existing hubspot companies
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>

        Task<long> UpdateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel);
    }
}
