using System.Collections.Generic;

namespace ClimateCamp.Infrastructure.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class HubSpotCompanySearchByDomainRequestModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FilterGroupsItem> filterGroups { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> properties { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public HubSpotCompanySearchByDomainRequestModel()
        {
            properties = new List<string> { "domain", "createddate", "name", "hs_lastmodifieddate", "production_volume",
                                            "address", "annualrevenue", "city", "climatecmap_id",
                                            "closedate", "country", "industry", "phone",
                                            "sustainability_targets", "zip", "numberofemployees"};
        }
    }

    public class FiltersItem
    {
        public string propertyName { get; set; }
        public string @operator { get; set; }
        public string value { get; set; }
    }

    public class FilterGroupsItem
    {
        public List<FiltersItem> filters { get; set; }
    }

    public class HubSpotCompanySearchByDomainResponseModel
    {
        public int total { get; set; }
        public List<Result> results { get; set; }

        public HubSpotCompanySearchByDomainResponseModel()
        {
            total = 0;
            results = new List<Result>();
        }
    }

    public class Result
    {
        public string id { get; set; }
        public Properties properties { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public bool archived { get; set; }
    }

    public class Properties
    {
        public string address { get; set; }
        public decimal annualrevenue { get; set; }
        public string city { get; set; }
        public string closedate { get; set; }
        public string country { get; set; }
        public string createdate { get; set; }
        public string domain { get; set; }
        public string hs_lastmodifieddate { get; set; }
        public string hs_object_id { get; set; }
        public string industry { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string production_volume { get; set; }
        public string sustainability_targets { get; set; }
        public string zip { get; set; }
        public string numberofemployees { get; set; }
    }

    public class HubSpotCompanyRequestModel // Getting the organizational data from the Organization Screen Self Service Wizard
    {
        public long HubSpotId { get; set; }
        public string name { get; set; }
        public int? numberofemployees { get; set; }
        public decimal annualrevenue { get; set; }
        public long? production_volume { get; set; }
        //TODO: Bug 816 fix needs to be applied around here.
        //public string climatecamp_id { get; set; }
        
    }

    public class HubSpotCompanyUpdateRequestModel
    {
        public Properties properties { get; set; }

        public HubSpotCompanyUpdateRequestModel()
        {
            properties = new Properties();
        }
    }


    public class HubSpotCompanyCreateOrUpdateResponseModel
    {
        public long id { get; set; }
    }
}
