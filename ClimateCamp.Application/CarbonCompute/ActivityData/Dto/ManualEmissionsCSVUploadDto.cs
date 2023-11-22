using CsvHelper.Configuration;
using FileHelpers;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    [DelimitedRecord(",")]
    public class ManualEmissionsCSVUploadDto
    {
        /// <summary>
        /// The Guid of the Organization Unit.
        /// </summary>
        public string OrganizationUnitId { get; set; }

        /// <summary>
        /// name of the Activity Data.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the Activity Type.
        /// </summary>
        public string ActivityTypeId { get; set; }

        /// <summary>
        /// What kind of data it is? What is the source of the data?
        /// </summary>
        public string DataQualityType { get; set; }

        /// <summary>
        /// From which date the activity data is for.
        /// </summary>
        public string ConsumptionStart { get; set; }

        /// <summary>
        /// Until what date the activity data is for
        /// </summary>
        public string ConsumptionEnd { get; set; }

        /// <summary>
        /// Can be billing date or ConsumptionEnd date. <br/>
        /// In some cases we use this along with other params to prevent duplicate entries.
        /// </summary>
        public string TransactionDate { get; set; }

        /// <summary>
        /// For this specific case we agreed to set it to 0 unless we actually know it's value. <br/>
        /// It's part of the classic/special differentiation mechanism.
        /// </summary>
        public string ActivityDataQuantity { get; set; }

        /// <summary>
        /// The Unit for the Quantity of Activity Data
        /// </summary>
        public string ActivityDataUnit { get; set; }

        /// <summary>
        /// We need to set this to a specific User Id. <br/>
        /// This is part of the classic/special differentiation mechanism
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EmissionsDataQualityScore { get; set; }

        /// <summary>
        /// Represents the total CO2E for the specific emission
        /// </summary>
        public string CO2e { get; set; }

        /// <summary>
        /// Represents the Unit name (kg, t, etc) for the CO2e.
        /// </summary>
        public string CO2eUnit { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ManualEmissionsCSVUploadModelMap : ClassMap<ManualEmissionsCSVUploadDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManualEmissionsCSVUploadModelMap()
        {
            Map(m => m.OrganizationUnitId).Name("organizationUnitId");
            Map(m => m.Name).Name("name");
            Map(m => m.ActivityTypeId).Name("activityTypeId");
            Map(m => m.DataQualityType).Name("dataQualityType");
            Map(m => m.ConsumptionStart).Name("consumptionStart");
            Map(m => m.ConsumptionEnd).Name("consumptionEnd");
            Map(m => m.TransactionDate).Name("transactionDate");
            Map(m => m.ActivityDataQuantity).Name("activityDataQuantity");
            Map(m => m.ActivityDataUnit).Name("activityDataUnit");
            Map(m => m.CreatorUserId).Name("creatorUserId");
            Map(m => m.EmissionsDataQualityScore).Name("emissionsDataQualityScore");
            Map(m => m.CO2e).Name("CO2e");
            Map(m => m.CO2eUnit).Name("CO2eUnit");
        }
    }

}
