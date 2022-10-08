using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentFailedIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
