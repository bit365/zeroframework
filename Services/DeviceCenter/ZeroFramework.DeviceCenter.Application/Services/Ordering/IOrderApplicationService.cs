using ZeroFramework.DeviceCenter.Application.Models.Ordering;

namespace ZeroFramework.DeviceCenter.Application.Services.Ordering
{
    public interface IOrderApplicationService
    {
        Task<List<OrderListResponseModel>> GetListAsync(OrderListRequestModel model, CancellationToken cancellationToken = default);

        Task<bool> CreateAsync(OrderCreateRequestModel model, CancellationToken cancellationToken = default);
    }
}