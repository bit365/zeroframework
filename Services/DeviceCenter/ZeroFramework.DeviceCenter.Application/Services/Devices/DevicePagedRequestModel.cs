using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public class DevicePagedRequestModel : PagedRequestModel
    {
        public string? Name { get; set; }

        public DeviceStatus? Status { get; set; }

        public Guid? ProductId { get; set; }

        public int? DeviceGroupId { get; set; }
    }
}
