using MediatR;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Events.Ordering
{
    /// <summary>
    /// Event used when an order is created
    /// </summary>
    public class OrderStartedDomainEvent(Order order, Guid userId, int cardTypeId, string cardNumber, DateTimeOffset cardExpiration) : INotification
    {
        public Guid UserId { get; } = userId;

        public int CardTypeId { get; } = cardTypeId;

        public string CardNumber { get; } = cardNumber;

        public DateTimeOffset CardExpiration { get; } = cardExpiration;

        public Order Order { get; } = order;
    }
}
