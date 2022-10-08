using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public interface IProductApplicationService
    {
        Task<ProductGetResponseModel> CreateAsync(ProductCreateRequestModel requestModel);

        Task DeleteAsync(Guid id);

        Task<ProductGetResponseModel> UpdateAsync(Guid id, ProductUpdateRequestModel requestModel);

        Task<ProductGetResponseModel> GetAsync(Guid id);

        Task<PagedResponseModel<ProductGetResponseModel>> GetListAsync(ProductPagedRequestModel requestModel);
    }
}