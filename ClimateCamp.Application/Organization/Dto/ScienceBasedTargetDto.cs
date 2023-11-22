using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ClimateCamp.Core;
using System;

namespace ClimateCamp.Application
{
    [AutoMapFrom(typeof(ScienceBasedTarget))]
    public class ScienceBasedTargetDto : EntityDto<Guid>
    {
        public int? BaseLineYear { get; set; }
        public int? SBTI { get; set; }
        public int? NearTermTarget { get; set; }
        public int? NearTermTargetYear { get; set; }
        public int? LongTermTarget { get; set; }
        public int? LongTermTargetYear { get; set; }
        public int? NetZeroCommitted { get; set; }
        public int? NetZeroYear { get; set; }
        public Guid OrganizationTargetId { get; set; }
    }
}
