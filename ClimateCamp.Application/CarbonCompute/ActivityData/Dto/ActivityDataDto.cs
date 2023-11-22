using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.CarbonCompute;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ActivityData))]
    public class ActivityDataDto : EntityDto<Guid>
    {
        public Guid? OrganizationUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }
        public bool IsActive { get; set; }
        public GHG.DataQualityType DataQualityType { get; set; }
        public DateTime ConsumptionStart { get; set; }
        public DateTime ConsumptionEnd { get; set; }
        public DateTime TransactionDate { get; set; }
        public float Quantity { get; set; }
        public int UnitId { get; set; }
        public int IndustrialProcessId { get; set; }
        public bool isProcessed { get; set; }
        public string TransactionId { get; set; }
        public string Evidence { get; set; }
        public string SourceTransactionId { get; set; }
        public string ConnectorId { get; set; }
        /// <summary>
        /// activity data status:
        /// <see cref="Core.CarbonCompute.Enum.ActivityDataStatus"/>
        /// </summary>
        public int Status { get; set; }
    }
}
