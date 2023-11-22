using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Notifications;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimateCamp.Application
{
    /// <summary>
    /// 
    /// </summary>
    [AbpAuthorize]
    public class NotificationAppService : CommonAppServiceBase, INotificationAppService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly INotificationStore _notificationStore;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationPublisher"></param>
        /// <param name="notificationStore"></param>
        /// <param name="onlineClientManager"></param>
        /// <param name="notificationDefinitionManager"></param>
        /// <param name="notificationSubscriptionManager"></param>
        public NotificationAppService(
            INotificationPublisher notificationPublisher,
            INotificationStore notificationStore,
            IOnlineClientManager onlineClientManager,
            INotificationDefinitionManager notificationDefinitionManager,
            INotificationSubscriptionManager notificationSubscriptionManager
            )
        {
            _notificationStore = notificationStore;
            _notificationPublisher = notificationPublisher;
            _onlineClientManager = onlineClientManager;
            _notificationDefinitionManager = notificationDefinitionManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        /// <inheritdoc/>
        public async Task DeleteAllUserNotifications(DeleteAllUserNotificationsInput body)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();

            await _notificationStore.DeleteAllUserNotificationsAsync(userIdentifier, body.State, body.StartDate, body.EndDate);
        }

        /// <inheritdoc/>
        public async Task DeleteNotification(EntityDto<Guid> body)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();

            if (body.Id == Guid.Empty)
            {
                throw new UserFriendlyException("Notification id is invalid.");
            }

            await _notificationStore.DeleteUserNotificationAsync(userIdentifier.TenantId, body.Id);


            Logger.Info("DeleteNotification");
        }

        /// <inheritdoc/>
        public async Task<GetNotificationsOutput> GetUserNotifications(GetNotificationsInput request)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();

            var result = new GetNotificationsOutput
            {
                TotalCount = await _notificationStore.GetUserNotificationCountAsync(userIdentifier, request.State, request.StartDate, request.EndDate),
                UnreadCount = await _notificationStore.GetUserNotificationCountAsync(userIdentifier, UserNotificationState.Unread, request.StartDate, request.EndDate)
            };

            List<UserNotificationInfoWithNotificationInfo> notifications
                = await _notificationStore.GetUserNotificationsWithNotificationsAsync(
                    userIdentifier, request.State, request.SkipCount, request.MaxResultCount, request.StartDate, request.EndDate);

            result.Items = notifications.Select(n => n.ToUserNotification()).ToList();

            return result;
        }

        /// <inheritdoc/>
        public async Task SetAllNotificationsAsRead()
        {
            var userIdentifier = AbpSession.ToUserIdentifier();

            await _notificationStore.UpdateAllUserNotificationStatesAsync(userIdentifier, UserNotificationState.Read);

            Logger.Info("SetAllNotificationsAsRead");
        }

        /// <inheritdoc/>
        public async Task<SetNotificationAsReadOutput> SetNotificationAsRead(EntityDto<Guid> body)
        {

            var userNotificationId = body.Id;

            if (userNotificationId == Guid.Empty)
            {
                throw new UserFriendlyException("Notification id is invalid.");
            }

            var result = new SetNotificationAsReadOutput();

            try
            {
                await _notificationStore.UpdateUserNotificationStateAsync(AbpSession.TenantId, userNotificationId, UserNotificationState.Read);
                result.Success = true;
                Logger.Info("SetNotificationAsRead");
            }
            catch (Exception ex)
            {
                result.Success = false;
                Logger.Error("SetNotificationAsRead", ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            var userIdentifier = AbpSession.ToUserIdentifier();

            var result = new GetNotificationSettingsOutput
            {
                ReceiveNotifications = true
            };

            //Getting all available definitions
            var notificationDefinitions = await _notificationDefinitionManager.GetAllAvailableAsync(userIdentifier);
            result.Notifications = notificationDefinitions.Select(d => new NotificationSubscriptionWithDisplayNameDto
            {
                Name = d.Name,
                //Description = d.Description,
                DisplayName = d.Name//d.DisplayName
            }).ToList();

            //Getting user's subscriptions
            List<NotificationSubscription> subscriptions = (await _notificationStore.GetSubscriptionsAsync(userIdentifier)).Select(x => x.ToNotificationSubscription()).ToList();
            foreach (var s in subscriptions)
            {
                //TODO: foreach active subsctiption, update the the IsSubscribed property of items in the 'result' variable.
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput body)
        {
            throw new UserFriendlyException("Updating the Notifications Settings feature is not available yet, stay tuned it's coming soon.");
        }
    }


}
