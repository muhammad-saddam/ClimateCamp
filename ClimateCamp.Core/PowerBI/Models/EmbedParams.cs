
using Microsoft.PowerBI.Api.Models;
using System.Collections.Generic;

namespace ClimateCamp.PowerBI.Models
{
    public class EmbedParams
    {
        public string Type { get; set; }

        // Report to be embedded
        public List<EmbedReport> EmbedReport { get; set; }

        // Embed Token for the Power BI report
        public EmbedToken EmbedToken { get; set; }
    }
}
