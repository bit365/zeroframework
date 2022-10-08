using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public class DeviceGroupPagedRequestModel : PagedRequestModel
    {
        public string? Keyword { get; set; }

        public int? ParentId { get; set; }
    }
}
