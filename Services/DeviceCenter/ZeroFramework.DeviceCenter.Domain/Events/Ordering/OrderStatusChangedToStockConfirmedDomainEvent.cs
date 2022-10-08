using MediatR;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    /// <summary>
    /// Event used when the order stock items are confirmed
    /// </summary>
    public class OrderStatusChangedToStockConfirmedDomainEvent : INotification
    {
        public Guid OrderId { get; }

        public OrderStatusChangedToStockConfirmedDomainEvent(Guid orderId) => OrderId = orderId;
    }
}