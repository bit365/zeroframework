using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.Events.Ordering;
using ZeroFramework.DeviceCenter.Domain.Exceptions;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate
{
    public class Order : BaseAggregateRoot<Guid>
    {
        private readonly List<OrderItem> _orderItems = [];

        public OrderStatus OrderStatus { get; private set; } = OrderStatus.Submitted;

        public ShippingAddress Address { get; private set; } = new ShippingAddress();

        public DateTimeOffset CreationTime { get; private set; } = DateTimeOffset.Now;

        public Guid BuyerId { get; private set; }

        public int? PaymentMethodId { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        protected Order() { }

        public Order(Guid buyerId, ShippingAddress address, int cardTypeId, string cardNumber, DateTimeOffset cardExpiration)
        {
            BuyerId = buyerId;
            Address = address;
            OrderStatus = OrderStatus.Submitted;

            // Add the OrderStarterDomainEvent to the domain events collection 
            // to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
            var domainEvent = new OrderStartedDomainEvent(this, buyerId, cardTypeId, cardNumber, cardExpiration);
            AddDomainEvent(domainEvent);
        }

        public decimal Total => _orderItems.Sum(o => o.Units * o.UnitPrice);

        public void SetPaymentMethodId(int id) => PaymentMethodId = id;

        public void SetBuyerId(Guid id) => BuyerId = id;

        public void AddOrderItem(int productId, decimal unitPrice, int units = 1)
        {
            var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId).SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                //if previous line exist modify it with higher discount  and units..
                if (unitPrice * units > sbyte.MaxValue)
                {
                    units += 1;
                }
                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                //add validated new order item
                var orderItem = new OrderItem(productId, unitPrice, units);
                _orderItems.Add(orderItem);
            }
        }

        public void SetPaidStatus()
        {
            if (OrderStatus == OrderStatus.StockConfirmed)
            {
                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));
                OrderStatus = OrderStatus.Paid;
            }
        }

        public void SetCancelledStatus()
        {
            if (OrderStatus == OrderStatus.Paid || OrderStatus == OrderStatus.Shipped)
            {
                throw new OrderingDomainException($"Is not possible to change the order status.");
            }
            OrderStatus = OrderStatus.Cancelled;
            AddDomainEvent(new OrderCancelledDomainEvent(this));
        }
    }
}