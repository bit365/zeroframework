using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    public class OrderShippedDomainEvent(Order order) : INotification
    {
        public Order Order { get; } = order;
    }
}
