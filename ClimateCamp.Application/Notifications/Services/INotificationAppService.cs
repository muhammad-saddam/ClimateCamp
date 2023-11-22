using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// Get the user notifications that were addressed to the logged in user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<GetNotificationsOutput> GetUserNotifications(GetNotificationsInput request);

        /// <summary>
        /// Set all the user notifications as read, for the logged in user
        /// </summary>
        /// <returns></returns>
        Task SetAllNotificationsAsRead();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> body);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput body);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Task DeleteNotification(EntityDto<Guid> body);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput body);

    }
}
