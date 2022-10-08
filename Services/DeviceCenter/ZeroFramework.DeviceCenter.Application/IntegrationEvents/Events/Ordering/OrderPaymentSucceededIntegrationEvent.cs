using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSucceededIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
