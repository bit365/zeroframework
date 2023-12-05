using MediatR;
using Microsoft.Extensions.Logging;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Events.Buyers;

namespace ZeroFramework.DeviceCenter.Application.DomainEventHandlers.Buyers.BuyerAndPaymentMethodVerified
{
    public class UpdateOrderWhenVerifiedDomainEventHandler(IOrderRepository orderRepository, ILoggerFactory loggerFactory) : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

        private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        /// <summary>
        /// When the Buyer and Buyer's payment method have been created or verified that they existed,
        /// then we can update the original Order with the BuyerId and PaymentId (foreign keys)
        /// </summary>
        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
        {
            Order orderToUpdate = await _orderRepository.GetAsync(buyerPaymentMethodVerifiedEvent.OrderId);

            orderToUpdate.SetBuyerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
            orderToUpdate.SetPaymentMethodId(buyerPaymentMethodVerifiedEvent.Payment.Id);

            var logger = _loggerFactory.CreateLogger<UpdateOrderWhenVerifiedDomainEventHandler>();
            string message = "Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})";
            logger.LogTrace(message, buyerPaymentMethodVerifiedEvent.OrderId, nameof(buyerPaymentMethodVerifiedEvent.Payment), buyerPaymentMethodVerifiedEvent.Payment.Id);
        }
    }
}