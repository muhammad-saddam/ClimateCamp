using Abp.Notifications;
using System.Collections.Generic;

namespace ClimateCamp.Application
{
    /// <summary>
    /// Response with Notifications for the logged in user.
    /// </summary>
    public class GetNotificationsOutput
    {
        /// <summary>
        /// Unread notifications count
        /// </summary>
        public int UnreadCount { get; set; }

        /// <summary>
        /// Total count of notifications
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// List of  user notifications
        /// </summary>
        public List<UserNotification> Items { get; set; }
    }
}