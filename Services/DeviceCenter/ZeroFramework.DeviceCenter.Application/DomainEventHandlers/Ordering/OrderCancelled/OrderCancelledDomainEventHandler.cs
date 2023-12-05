using MediatR;
using Microsoft.Extensions.Logging;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;

namespace ZeroFramework.DeviceCenter.Application.DomainEventHandlers.Ordering.OrderCancelled
{
    public class OrderCancelledDomainEventHandler(IOrderRepository orderRepository, ILoggerFactory loggerFactory, IBuyerRepository buyerRepository, IIntegrationEventService integrationEventService) : INotificationHandler<OrderCancelledDomainEvent>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

        private readonly IBuyerRepository _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));

        private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        private readonly IIntegrationEventService _integrationEventService = integrationEventService;

        public async Task Handle(OrderCancelledDomainEvent orderCancelledDomainEvent, CancellationToken cancellationToken)
        {
            _loggerFactory.CreateLogger<OrderCancelledDomainEvent>().LogTrace("Order with Id: {OrderId} has been successfully updated to status {Status} ({Id})", orderCancelledDomainEvent.Order.Id, nameof(OrderStatus.Cancelled), OrderStatus.Cancelled.Id);

            var order = await _orderRepository.GetAsync(orderCancelledDomainEvent.Order.Id);
            Buyer? buyer = await _buyerRepository.FindByIdAsync(order.BuyerId);

            if (buyer is not null)
            {
                var orderStatusChangedToCancelledIntegrationEvent = new OrderStatusChangedToCancelledIntegrationEvent(order.Id, order.OrderStatus.Name, buyer.UserId);

                await _integrationEventService.AddAndSaveEventAsync(orderStatusChangedToCancelledIntegrationEvent);
            }
        }
    }
}
