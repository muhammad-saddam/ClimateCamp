using Abp.Notifications;
using System;

namespace ClimateCamp.Application
{
    [Serializable]
    public class FileUploadedNotificationData : NotificationData
    {
        public string FileName { get; set; }

        public string FileUploadedMessage { get; set; }

        public FileUploadedNotificationData(string fileName)
        {
            FileName = fileName;
            FileUploadedMessage = "File uploaded.";
        }
    }
}
