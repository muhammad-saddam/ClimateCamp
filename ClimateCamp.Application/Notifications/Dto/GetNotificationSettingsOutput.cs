using System.Collections.Generic;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class GetNotificationSettingsOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}