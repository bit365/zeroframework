using ZeroFramework.DeviceCenter.Domain.Constants;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate
{
    public interface IDeviceRepository : IRepository<Device, long>
    {
        Task<List<Device>> GetListAsync(int? productId, int? deviceGroupId, DeviceStatus? status, string? deviceName, int pageNumber = 1, int pageSize = PagingConstants.DefaultPageSize, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(int? productId, int? deviceGroupId, DeviceStatus? status, string? deviceName, CancellationToken cancellationToken = default);
    }
}