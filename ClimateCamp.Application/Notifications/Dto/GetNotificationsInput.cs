using Abp.Application.Services.Dto;
using Abp.Notifications;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class GetNotificationsInput : PagedResultRequestDto
    {
        /// <summary>
        /// 
        /// </summary>
        public UserNotificationState? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}