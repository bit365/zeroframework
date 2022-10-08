using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.ResourceGroups
{
    public class ResourceGroupPagedRequestModel : PagedRequestModel
    {
        public string? Keyword { get; set; }
    }
}
