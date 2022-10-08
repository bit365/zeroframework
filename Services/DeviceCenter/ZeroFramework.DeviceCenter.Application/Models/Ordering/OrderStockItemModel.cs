namespace ZeroFramework.DeviceCenter.Application.Models.Ordering
{
    public class OrderStockItemModel
    {
        public int ProductId { get; }

        public int Units { get; }

        public OrderStockItemModel(int productId, int units)
        {
            ProductId = productId;
            Units = units;
        }
    }
}
