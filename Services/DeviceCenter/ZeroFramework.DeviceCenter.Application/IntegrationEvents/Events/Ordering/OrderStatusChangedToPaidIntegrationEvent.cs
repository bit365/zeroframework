using ZeroFramework.DeviceCenter.Application.Models.Ordering;
using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering
{
    public class OrderStatusChangedToPaidIntegrationEvent(int orderId, string orderStatus, Guid buyerId, IEnumerable<OrderStockItemModel> orderStockItems) : IntegrationEvent
    {
        public int OrderId { get; } = orderId;

        public string OrderStatus { get; } = orderStatus;

        public Guid BuyerId { get; } = buyerId;

        public IEnumerable<OrderStockItemModel> OrderStockItems { get; } = orderStockItems;
    }
}
