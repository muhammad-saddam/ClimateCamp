namespace ClimateCamp.Core.Notifications
{
    /// <summary>
    /// Strongly typed notifications types definition
    /// </summary>
    public static class NotificationTypes
    {
        /// <summary>
        /// To notify when new activity data provided by the user has been uploaded
        /// </summary>
        public const string FileUploaded = "ClimateCamp.FileUploaded";

        /// <summary>
        /// To notify when the emissions calculation for a user / organization has been uploaded
        /// </summary>
        public const string CalculationCompleted = "ClimateCamp.CalculationCompleted";

        /// <summary>
        /// To notify the organization administrators that a new user from the organization registered on the platform
        /// </summary>
        public const string NewOrganizationUser = "ClimateCamp.NewUserRegistered";

        /// <summary>
        /// To notify users about anouncements and news from ClimateCamp
        /// </summary>
        public const string News = "ClimateCamp.News";
    }
}
