﻿using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.Events.Buyers;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate
{
    public class Buyer(Guid userId) : BaseAggregateRoot<Guid>
    {
        public Guid UserId { get; private set; } = userId;

        private readonly List<PaymentMethod> _paymentMethods = [];

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        public PaymentMethod VerifyOrAddPaymentMethod(string cardNumber, int cardType, DateTimeOffset expiration, Guid orderId)
        {
            var existingPayment = _paymentMethods.SingleOrDefault(p => p.CardNumber.Equals(cardNumber));

            if (existingPayment != null)
            {
                AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
                return existingPayment;
            }

            var payment = new PaymentMethod(cardNumber, cardType, expiration);

            _paymentMethods.Add(payment);

            AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

            return payment;
        }
    }
}
