using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    /// <summary>
    /// Event used when the grace period order is confirmed
    /// </summary>
    public class OrderStatusChangedToAwaitingValidationDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems) : INotification
    {
        public Guid OrderId { get; } = orderId;

        public IEnumerable<OrderItem> OrderItems { get; } = orderItems;
    }
}