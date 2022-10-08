using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Services.Devices
{
    public interface IDeviceGroupDomainService
    {
        Task<(List<DeviceGroup> Items, int TotalCount)> GetDeviceGroupListAsync(int? parentId, string? keyword, int pageNumber, int pageSize);
    }
}
