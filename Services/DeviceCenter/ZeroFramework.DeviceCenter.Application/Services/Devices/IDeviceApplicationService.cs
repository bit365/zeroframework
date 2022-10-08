using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public interface IDeviceApplicationService
    {
        Task<DeviceGetResponseModel> CreateAsync(DeviceCreateRequestModel requestModel);

        Task DeleteAsync(long id);

        Task<DeviceGetResponseModel> UpdateAsync(long id, DeviceUpdateRequestModel requestModel);

        Task<DeviceGetResponseModel> GetAsync(long id);

        Task<PagedResponseModel<DeviceGetResponseModel>> GetListAsync(DevicePagedRequestModel requestModel);

        Task<DeviceStatisticGetResponseModel> GetStatistics();
    }
}