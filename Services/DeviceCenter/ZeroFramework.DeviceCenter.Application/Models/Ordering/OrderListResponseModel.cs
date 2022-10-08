using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Ordering
{
    public class OrderListResponseModel
    {
        public Guid OrderId { get; set; }

        [AllowNull]
        public string OrderStatus { get; set; }

        public string Address { get; private set; } = string.Empty;

        public DateTimeOffset CreationTime { get; private set; }

        public Guid BuyerId { get; private set; }

        public int? PaymentMethodId { get; private set; }

        public IEnumerable<OrderItem> OrderItems = new List<OrderItem>();

        public class OrderItem
        {
            public int ProductId { get; set; }

            public decimal UnitPrice { get; set; }

            public int Units { get; set; }
        }
    }
}