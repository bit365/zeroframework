using ZeroFramework.DeviceCenter.Application.Models.Monitoring;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Queries.Monitoring
{
    public interface IMonitoringFactorQueries
    {
        Task<MonitoringFactorGetResponseModel> GetMonitoringFactorAsync(int id);

        Task<PagedResponseModel<MonitoringFactorGetResponseModel>> GetMonitoringFactorsAsync(MonitoringFactorPagedRequestModel model);
    }
}