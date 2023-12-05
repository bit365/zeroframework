using MediatR;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;
using ZeroFramework.EventBus.Abstractions;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.EventHandling.Ordering
{
    public class OrderPaymentSucceededDynamicIntegrationEventHandler(IMediator mediator) : IDynamicIntegrationEventHandler
    {
        private readonly IMediator _mediator = mediator;

        public async Task HandleAsync(dynamic eventData)
        {
            var command = new SetPaidOrderStatusCommand(eventData.OrderId);
            await _mediator.Send(command);
        }
    }
}