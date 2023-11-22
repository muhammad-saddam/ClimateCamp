using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapTo(typeof(DataCollection))]
    public class CreateDataCollectionDto : EntityDto<Guid>
    {
        public int Type { get; set; }
        public string DataSourceName { get; set; }
        public string Category { get; set; }
        public string Channel { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}

