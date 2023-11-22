using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using ClimateCamp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClimateCamp.Common.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : ClimateCampControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationPublisher"></param>
        public HomeController(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        /// <summary>
        /// TODO: This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;

            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 429);//admin@climatecamp.io in staging, in production Id is 1

            var vitalie = new UserIdentifier(null, 584);//vitalie+test1@climatecamp.io

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] {
                    defaultTenantAdmin,
                    hostAdmin,
                    vitalie }
            );

            return Content("Sent notification: " + message);
        }
    }
}
