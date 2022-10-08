using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public string OrderStatus { get; }

        public Guid BuyerId { get; }

        public OrderStatusChangedToSubmittedIntegrationEvent(Guid orderId, string orderStatus, Guid buyerId)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
            BuyerId = buyerId;
        }
    }
}
