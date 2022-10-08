using MediatR;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;

namespace ZeroFramework.DeviceCenter.Application.DomainEventHandlers.Ordering.OrderCancelled
{
    public class SendSmsWhenOrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
    {
        public Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}