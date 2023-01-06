using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Measurements
{
    public interface IDeviceDataApplicationService
    {
        Task<PageableListResposeModel<DevicePropertyValue>?> GetDevicePropertyHistoryValues(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, bool hoursFirst = false, SortingOrder sorting = SortingOrder.Ascending, int offset = 0, int count = 10);

        Task<PageableListResposeModel<DevicePropertyReport>?> GetDevicePropertyReports(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int offset = 0, int count = 10);

        Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(int productId, long deviceId);

        Task SetDevicePropertyValues(int productId, long deviceId, IDictionary<string, DevicePropertyValue> values);
    }
}
