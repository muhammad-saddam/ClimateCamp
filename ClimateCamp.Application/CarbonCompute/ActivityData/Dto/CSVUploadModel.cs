using CsvHelper.Configuration;
using FileHelpers;

namespace ClimateCamp.Application
{
    [DelimitedRecord(",")]
    public class CSVUploadModel
    {
        public string organizationUnit { get; set; }
        public string activityDataType { get; set; }
        public string quantity { get; set; }
        public string quantityUnit { get; set; }
        public string transactionDate { get; set; }
        public string transactionSource { get; set; }
        public string activityName { get; set; }
        public string activityDescription { get; set; }
    }

    public class CSVUploadModelMap : ClassMap<CSVUploadModel>
    {
        public CSVUploadModelMap()
        {
            Map(m => m.organizationUnit).Name("organizationUnit");
            Map(m => m.activityDataType).Name("activityDataType");
            Map(m => m.quantity).Name("quantity");
            Map(m => m.quantityUnit).Name("quantityUnit");
            Map(m => m.transactionDate).Name("transactionDate");
            Map(m => m.transactionSource).Name("transactionSource");
            Map(m => m.activityName).Name("activityName");
            Map(m => m.activityDescription).Name("activityDescription");
        }
    }

}
