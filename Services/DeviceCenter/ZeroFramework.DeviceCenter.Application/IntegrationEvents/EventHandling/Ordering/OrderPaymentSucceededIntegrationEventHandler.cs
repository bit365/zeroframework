using MediatR;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.EventBus.Abstractions;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.EventHandling.Ordering
{
    public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public OrderPaymentSucceededIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task HandleAsync(OrderPaymentSucceededIntegrationEvent @event)
        {
            var command = new SetPaidOrderStatusCommand(@event.OrderId);
            await _mediator.Send(command);
        }
    }
}