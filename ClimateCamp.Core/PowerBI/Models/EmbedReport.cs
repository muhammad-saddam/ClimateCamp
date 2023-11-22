using System;

namespace ClimateCamp.PowerBI.Models
{
    public class EmbedReport
    {
        public Guid ReportId { get; set; }

        // Name of the report
        public string ReportName { get; set; }

        // Embed URL for the Power BI report
        public string EmbedUrl { get; set; }
    }
}
