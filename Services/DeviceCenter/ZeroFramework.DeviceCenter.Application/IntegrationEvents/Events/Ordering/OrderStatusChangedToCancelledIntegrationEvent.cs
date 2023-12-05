using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderStatusChangedToCancelledIntegrationEvent(Guid orderId, string orderStatus, Guid buyerId) : IntegrationEvent
    {
        public Guid OrderId { get; } = orderId;

        public string OrderStatus { get; } = orderStatus;

        public Guid BuyerId { get; } = buyerId;
    }
}
