namespace ClimateCamp.Application
{
    public class EmissionsReportCSVModel
    {
        public string OrganizationUnit { get; set; }
        public string OrganizationUnitType { get; set; }
        public string Country { get; set; }
        public double? CO2E { get; set; }
        public string Unit { get; set; }
        public string EmissionScope { get; set; }
        public string EmissionSource { get; set; }
        public string ActivityDataType { get; set; }
        // public Guid ActivityDataId { get; set; }
        public float? Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionSource { get; set; }
        public string ActivityName { get; set; }
        public string ActivityDescription { get; set; }
        public string EmissionFactorLibrary { get; set; }
        public string EmissionFactorLibraryYear { get; set; }
    }
}
