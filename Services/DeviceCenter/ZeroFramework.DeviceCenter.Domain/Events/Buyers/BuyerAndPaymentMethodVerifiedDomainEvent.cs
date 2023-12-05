using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Buyers
{
    public class BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId) : INotification
    {
        public Buyer Buyer { get; private set; } = buyer;

        public PaymentMethod Payment { get; private set; } = payment;

        public Guid OrderId { get; private set; } = orderId;
    }
}
