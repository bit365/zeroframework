using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Queries.Monitoring
{
    public class MonitoringFactorPagedRequestModel : PagedRequestModel
    {
        public string? Keyword { get; set; }
    }
}
