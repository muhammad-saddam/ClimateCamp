

namespace Mobile.Combustion.Calculation.Models
{
    public class ServiceBusMessageModel
    {
        public string QueueName { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
