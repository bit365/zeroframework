using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Ordering
{
    public class OrderCreateRequestModel
    {
        public Guid OrderId { get; set; }

        [AllowNull]
        public string OrderStatus { get; set; }

        public string Address { get; private set; } = string.Empty;

        public DateTimeOffset CreationTime { get; private set; }

        public Guid BuyerId { get; private set; }

        public int? PaymentMethodId { get; private set; }
    }
}
