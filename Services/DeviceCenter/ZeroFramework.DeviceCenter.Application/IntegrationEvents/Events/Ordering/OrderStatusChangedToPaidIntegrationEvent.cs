using ZeroFramework.DeviceCenter.Application.Models.Ordering;
using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public string OrderStatus { get; }

        public Guid BuyerId { get; }

        public IEnumerable<OrderStockItemModel> OrderStockItems { get; }

        public OrderStatusChangedToPaidIntegrationEvent(int orderId, string orderStatus, Guid buyerId, IEnumerable<OrderStockItemModel> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
            OrderStatus = orderStatus;
            BuyerId = buyerId;
        }
    }
}
