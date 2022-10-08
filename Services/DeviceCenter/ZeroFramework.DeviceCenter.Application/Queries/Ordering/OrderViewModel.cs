namespace ZeroFramework.DeviceCenter.Application.Queries.Ordering
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public Guid BuyerId { get; set; }
    }
}