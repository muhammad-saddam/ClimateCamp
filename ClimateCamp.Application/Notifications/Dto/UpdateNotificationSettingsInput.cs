using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateNotificationSettingsInput
    {
        /// <summary>
        /// Used to completely Enable/Disable receiving notifications
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        /// <summary>
        /// Specific notification types subscription settings
        /// </summary>
        public List<NotificationSubscriptionDto> Notifications { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class NotificationSubscriptionDto
    {
        /// <summary>
        /// The setting for a specific notificaiton type. ex. <c>"ClimateCamp.FileUploaded"</c> or a value from the <c>NotificationTypes</c> class.
        /// </summary>
        [Required, StringLength(96)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSubscribed { get; set; }
    }
}