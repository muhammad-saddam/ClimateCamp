using System;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Application
{
    public class EmissionsReportFilterModel
    {
        //[Required]
        public Guid OrganizationId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        /// <summary>
        /// Select emissions by emissions source id (int).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? EmissionsSourceId { get; set; }
    }
}