using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public interface IDeviceGroupApplicationService
    {
        Task<DeviceGroupGetResponseModel> CreateAsync(DeviceGroupCreateRequestModel requestModel);

        Task DeleteAsync(int id);

        Task<DeviceGroupGetResponseModel> UpdateAsync(int id, DeviceGroupUpdateRequestModel requestModel);

        Task<DeviceGroupGetResponseModel> GetAsync(int id);

        Task<PagedResponseModel<DeviceGroupGetResponseModel>> GetListAsync(DeviceGroupPagedRequestModel requestModel);

        Task AddDevicesToGroup(int deviceGroupId, params long[] deviceIds);

        Task RemoveDevicesFromGroup(int deviceGroupId, params long[] deviceIds);
    }
}