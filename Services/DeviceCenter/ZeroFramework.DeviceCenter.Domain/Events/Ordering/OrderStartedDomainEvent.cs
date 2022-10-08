using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    /// <summary>
    /// Event used when an order is created
    /// </summary>
    public class OrderStartedDomainEvent : INotification
    {
        public Guid UserId { get; }

        public int CardTypeId { get; }

        public string CardNumber { get; }

        public DateTimeOffset CardExpiration { get; }

        public Order Order { get; }

        public OrderStartedDomainEvent(Order order, Guid userId, int cardTypeId, string cardNumber, DateTimeOffset cardExpiration)
        {
            Order = order;
            UserId = userId;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardExpiration = cardExpiration;
        }
    }
}
