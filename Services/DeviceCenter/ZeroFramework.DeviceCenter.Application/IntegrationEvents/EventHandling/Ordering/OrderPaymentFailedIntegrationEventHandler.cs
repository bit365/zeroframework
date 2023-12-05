using MediatR;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.EventBus.Abstractions;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.EventHandling.Ordering
{
    public class OrderPaymentFailedIntegrationEventHandler(IMediator mediator) : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        public async Task HandleAsync(OrderPaymentFailedIntegrationEvent @event)
        {
            var command = new CancelOrderCommand(@event.OrderId);
            await _mediator.Send(command);
        }
    }
}
