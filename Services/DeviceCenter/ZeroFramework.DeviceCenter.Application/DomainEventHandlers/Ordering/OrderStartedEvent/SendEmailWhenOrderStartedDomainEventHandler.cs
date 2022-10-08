using MediatR;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;

namespace ZeroFramework.DeviceCenter.Application.DomainEventHandlers.Ordering.OrderStartedEvent
{
    public class SendEmailWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        public Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
