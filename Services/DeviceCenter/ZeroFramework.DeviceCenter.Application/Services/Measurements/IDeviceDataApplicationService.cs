using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Measurements
{
    public interface IDeviceDataApplicationService
    {
        Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(Guid productId, long deviceId);

        Task<PageableListResposeModel<DevicePropertyValue>> GetDevicePropertyHistoryValues(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, SortingOrder sorting, int skip, int top);

        Task<PageableListResposeModel<DevicePropertyReport>> GetDevicePropertyReports(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int skip, int top);

        Task SetDevicePropertyValue(Guid productId, long deviceId, string identifier, DevicePropertyValue propertyValue);
    }
}
