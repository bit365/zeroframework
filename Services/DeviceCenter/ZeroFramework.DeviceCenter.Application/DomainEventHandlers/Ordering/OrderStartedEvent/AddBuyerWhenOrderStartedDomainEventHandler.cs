using MediatR;
using System.Transactions;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;

namespace ZeroFramework.DeviceCenter.Application.DomainEventHandlers.Ordering.OrderStartedEvent
{
    public class AddBuyerWhenOrderStartedDomainEventHandler(IBuyerRepository buyerRepository, IIntegrationEventService integrationEventService) : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly IBuyerRepository _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));

        private readonly IIntegrationEventService _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));

        public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
        {
            Buyer? buyer = await _buyerRepository.FindAsync(orderStartedEvent.UserId);

            bool buyerOriginallyExisted = buyer != null;

            buyer ??= new Buyer(userId: orderStartedEvent.UserId);

            buyer.VerifyOrAddPaymentMethod(orderStartedEvent.CardNumber, orderStartedEvent.CardTypeId, orderStartedEvent.CardExpiration, orderStartedEvent.Order.Id);

            _ = buyerOriginallyExisted ? _buyerRepository.Update(buyer) : _buyerRepository.Add(buyer);

            var integrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(orderStartedEvent.Order.Id, orderStartedEvent.Order.OrderStatus.Name, buyer.UserId);

            using var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            await _buyerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await _integrationEventService.AddAndSaveEventAsync(integrationEvent);
            // Commit transaction if all commands succeed, transaction will auto-rollback
            // when disposed if either commands fails
            scope.Complete();
        }
    }
}