using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.Exceptions;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate
{
    public class OrderStatus : Enumeration
    {
        public static OrderStatus Submitted { get; } = new OrderStatus(1, nameof(Submitted).ToLowerInvariant());

        public static OrderStatus AwaitingValidation { get; } = new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());

        public static OrderStatus StockConfirmed { get; } = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());

        public static OrderStatus Paid { get; } = new OrderStatus(4, nameof(Paid).ToLowerInvariant());

        public static OrderStatus Shipped { get; } = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());

        public static OrderStatus Cancelled { get; } = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

        public OrderStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<OrderStatus> List() => new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };

        public static OrderStatus FromName(string name)
        {
            var state = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static OrderStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for OrderStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
