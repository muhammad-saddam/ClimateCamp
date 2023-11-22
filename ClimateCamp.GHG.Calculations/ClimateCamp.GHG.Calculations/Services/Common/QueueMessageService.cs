
using Azure.Messaging.ServiceBus;
using Mobile.Combustion.Calculation.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services.Common
{
    public class QueueMessageService : IQueueMessageService
    {
        static ServiceBusClient client;
        static ServiceBusSender queueClient;
        //private string connectionString = "Endpoint=sb://sb-climatecamp-communication-bus-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AjzY+t/J/LbYyjAZS8zQyBS4l2kl57chKU7Jv2NfqdY=";
        public async Task<bool> PushMessageToQueue(ServiceBusMessageModel message)
        {
            client = new ServiceBusClient(Environment.GetEnvironmentVariable("ServiceBusConnectionString"));
            var queueClient = client.CreateSender(message.QueueName);

            try
            {
                var serializeBody = JsonConvert.SerializeObject(message);

                // send data to bus
                ServiceBusMessage busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(serializeBody));
                busMessage.ApplicationProperties.Add("Type", message.Type);
                busMessage.MessageId = message.Id;
                await queueClient.SendMessageAsync(busMessage);
            }
            catch (Exception e)
            {

            }
            finally
            {
                await queueClient.CloseAsync();
            }
            return false;
        }

    }
}
