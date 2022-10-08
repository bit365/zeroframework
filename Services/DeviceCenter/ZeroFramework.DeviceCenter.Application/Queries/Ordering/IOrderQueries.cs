namespace ZeroFramework.DeviceCenter.Application.Queries.Ordering
{
    public interface IOrderQueries
    {
        Task<OrderViewModel> GetOrderAsync(Guid id);
    }
}
