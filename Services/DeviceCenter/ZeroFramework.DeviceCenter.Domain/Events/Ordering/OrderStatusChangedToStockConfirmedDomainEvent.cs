using MediatR;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    /// <summary>
    /// Event used when the order stock items are confirmed
    /// </summary>
    public class OrderStatusChangedToStockConfirmedDomainEvent(Guid orderId) : INotification
    {
        public Guid OrderId { get; } = orderId;
    }
}