using Mobile.Combustion.Calculation.Models;
using System.Threading.Tasks;

namespace Mobile.Combustion.Calculation.Services.Common
{
    public interface IQueueMessageService
    {
        Task<bool> PushMessageToQueue(ServiceBusMessageModel message);
    }
}
