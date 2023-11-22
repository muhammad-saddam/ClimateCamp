using CsvHelper.Configuration;
using FileHelpers;

namespace ClimateCamp.Application
{

    /// <summary>
    /// 
    /// </summary>
    [DelimitedRecord(",")]
    public class StationaryCombustionCSVUploadDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationUnit { get; set; }
        /// <summary>
        /// activity dat atype like Purchased Natural Gas
        /// </summary>
        public string ActivityDataType { get; set; }
        /// <summary>
        /// actual quantity of data
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// unit of activity like kwh, EUR
        /// </summary>
        public string QuantityUnit { get; set; }
        /// <summary>
        /// date of activity
        /// </summary>
        public string TransactionDate { get; set; }
        /// <summary>
        /// source of activity data
        /// </summary>
        public string TransactionSource { get; set; }
        /// <summary>
        /// activity name 
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// description of activity 
        /// </summary>
        public string ActivityDescription { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StationaryCombustionCSVUploadDtoMap : ClassMap<StationaryCombustionCSVUploadDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public StationaryCombustionCSVUploadDtoMap()
        {
            Map(m => m.OrganizationUnit).Name("organizationUnit");
            Map(m => m.ActivityDataType).Name("activityDataType");
            Map(m => m.Quantity).Name("quantity");
            Map(m => m.QuantityUnit).Name("quantityUnit");
            Map(m => m.TransactionDate).Name("transactionDate");
            Map(m => m.TransactionSource).Name("transactionSource");
            Map(m => m.ActivityName).Name("activityName");
            Map(m => m.ActivityDescription).Name("activityDescription");
        }
    }
}

