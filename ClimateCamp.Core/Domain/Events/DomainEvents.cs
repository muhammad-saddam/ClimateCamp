using Abp.Events.Bus;

namespace ClimateCamp.Domain.Events
{
    /// <summary>
    /// Register the EventBus to capture triggers and event handlers https://aspnetboilerplate.com/Pages/Documents/EventBus-Domain-Events
    /// </summary>
    public static class DomainEvents
    {
        public static IEventBus EventBus { get; set; }

        static DomainEvents()
        {
            EventBus = Abp.Events.Bus.EventBus.Default;
            //EventBus = Abp.Events.Bus.NullEventBus.Instance;
        }
    }
}
