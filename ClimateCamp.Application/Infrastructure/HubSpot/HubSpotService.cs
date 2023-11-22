using Abp;
using Abp.Dependency;
using ClimateCamp.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ClimateCamp.Infrastructure.HubSpot
{
    /// <summary>
    /// HubSpot APi calling service
    /// </summary>
    public class HubSpotService : AbpServiceBase, IHubspotService, ITransientDependency
    {

        private readonly IConfiguration _config;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="config"></param>
        public HubSpotService(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Create HubSpot Company
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<long> CreateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel)
        {
            try
            {
                using (var client = new RestClient("https://api.hubapi.com/crm/v3/objects/companies"))
                {
                    var request = new RestRequest();
                    request.Method = Method.Post;
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("accept", "*/*");
                    request.AddHeader("Authorization", string.Format("Bearer {0}", _config.GetValue<string>("App:HubSpotAccessToken")));
                    request.AddJsonBody(hubSpotCompanyModel);
                    var result = await client.ExecutePostAsync<HubSpotCompanyCreateOrUpdateResponseModel>(request);
                    return result.IsSuccessful ? result.Data.id : 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to create HubSpot Company", ex);
                throw;
            }
        }

        /// <summary>
        /// get Hubspot company by email
        /// </summary>
        /// <param name="workEmail"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<HubSpotCompanySearchByDomainResponseModel> GetHubSpotCompanyByDomainAsync(string workEmail)
        {
            try
            {
                using (var client = new RestClient("https://api.hubapi.com/crm/v3/objects/companies/search"))
                {
                    var request = new RestRequest();

                    request.Method = Method.Post;

                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("accept", "*/*");
                    request.AddHeader("Authorization", string.Format("Bearer {0}", _config.GetValue<string>("App:HubSpotAccessToken")));

                    var hubSpotCompanySearchByDomainModel = new HubSpotCompanySearchByDomainRequestModel
                    {
                        filterGroups = new List<FilterGroupsItem>
                        {
                        new FilterGroupsItem
                        {
                             filters = new List<FiltersItem>{ new FiltersItem
                             {
                                 propertyName = "domain",
                                 @operator = "EQ",
                                 value = workEmail.Substring(workEmail.IndexOf("@") + 1)
                             }
                           }
                        }
                      }
                    };

                    request.AddJsonBody(hubSpotCompanySearchByDomainModel);

                    var result = await client.ExecutePostAsync<HubSpotCompanySearchByDomainResponseModel>(request);

                    return result.Data.total > 0 ? result.Data : new HubSpotCompanySearchByDomainResponseModel();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("HubSpot company retrieval failed", ex);
                throw;
            }
        }


        /// <summary>
        /// Update hubspot company info
        /// </summary>
        /// <param name="hubSpotCompanyModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<long> UpdateHubSpotCompany(HubSpotCompanyRequestModel hubSpotCompanyModel)
        {
            try
            {
                using (var client = new RestClient(string.Format("https://api.hubapi.com/crm/v3/objects/companies/{0}", hubSpotCompanyModel.HubSpotId)))
                {
                    var request = new RestRequest();
                    request.Method = Method.Patch;
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("accept", "*/*");
                    request.AddHeader("Authorization", string.Format("Bearer {0}", _config.GetValue<string>("App:HubSpotAccessToken")));
                    request.AddJsonBody(new HubSpotCompanyUpdateRequestModel { properties = { name = hubSpotCompanyModel.name, numberofemployees = Convert.ToString(hubSpotCompanyModel.numberofemployees ?? 0), annualrevenue = hubSpotCompanyModel.annualrevenue, production_volume = Convert.ToString(hubSpotCompanyModel.production_volume ?? 0) } });
                    var result = await client.ExecuteAsync<HubSpotCompanyCreateOrUpdateResponseModel>(request);
                    return result.IsSuccessful ? result.Data.id : 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to create HubSpot Company", ex);
                throw;
            }
        }
    }
}
