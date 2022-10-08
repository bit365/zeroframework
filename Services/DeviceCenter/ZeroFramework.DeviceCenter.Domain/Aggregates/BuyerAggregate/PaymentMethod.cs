using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate
{
    public class PaymentMethod : BaseEntity<int>
    {
        public string CardNumber { get; private set; } = string.Empty;

        public CardType CardType { get; private set; }

        public DateTimeOffset Expiration { get; private set; }

        protected PaymentMethod() { }

        public PaymentMethod(string cardNumber, int cardType, DateTimeOffset expiration)
        {
            CardNumber = cardNumber;
            CardType = (CardType)cardType;
            Expiration = expiration;
        }
    }
}
