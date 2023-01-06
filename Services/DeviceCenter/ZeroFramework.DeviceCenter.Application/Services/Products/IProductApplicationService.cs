using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public interface IProductApplicationService
    {
        Task<ProductGetResponseModel> CreateAsync(ProductCreateRequestModel requestModel);

        Task DeleteAsync(int id);

        Task<ProductGetResponseModel> UpdateAsync(int id, ProductUpdateRequestModel requestModel);

        Task<ProductGetResponseModel> GetAsync(int id);

        Task<PagedResponseModel<ProductGetResponseModel>> GetListAsync(ProductPagedRequestModel requestModel);
    }
}