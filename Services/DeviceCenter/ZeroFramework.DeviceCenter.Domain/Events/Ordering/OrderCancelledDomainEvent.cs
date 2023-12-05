using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    public class OrderCancelledDomainEvent(Order order) : INotification
    {
        public Order Order { get; } = order;
    }
}
