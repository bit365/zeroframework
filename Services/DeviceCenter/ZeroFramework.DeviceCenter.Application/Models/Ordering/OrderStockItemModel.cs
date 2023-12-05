namespace ZeroFramework.DeviceCenter.Application.Models.Ordering
{
    public class OrderStockItemModel(int productId, int units)
    {
        public int ProductId { get; } = productId;

        public int Units { get; } = units;
    }
}
