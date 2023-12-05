using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderPaymentSucceededIntegrationEvent(Guid orderId) : IntegrationEvent
    {
        public Guid OrderId { get; } = orderId;
    }
}
