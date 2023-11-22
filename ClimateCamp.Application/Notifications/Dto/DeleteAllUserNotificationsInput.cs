using Abp.Notifications;
using System;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Used to select what notifications to request for deletion
    /// </summary>
    public class DeleteAllUserNotificationsInput
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