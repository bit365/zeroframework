using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public class ProductPagedRequestModel : PagedRequestModel
    {
        public string? Keyword { get; set; }
    }
}
