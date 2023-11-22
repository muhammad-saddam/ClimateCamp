using Abp.Localization;
using Abp.Notifications;

namespace ClimateCamp.Core.Notifications
{
    /// <summary>
    /// ClimateCamp notification provider for defining all notification types.
    /// </summary>
    public class ClimateCampAppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    NotificationTypes.News,
                    displayName: new LocalizableString("NewsNotificationDefinition", ClimateCampConsts.LocalizationSourceName), //"ClimateCamp news and announcements"
                    permissionDependency: null//new SimplePermissionDependency("App.Pages.UserManagement")
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    NotificationTypes.FileUploaded,
                    displayName: new LocalizableString("FileUploadedNotificationDefinition", ClimateCampConsts.LocalizationSourceName),
                    permissionDependency: null//new SimplePermissionDependency("App.Pages.UserManagement")
                    )
                );
        }
    }
}