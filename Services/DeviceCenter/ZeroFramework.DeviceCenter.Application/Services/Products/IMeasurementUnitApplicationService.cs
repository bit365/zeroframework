using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public interface IMeasurementUnitApplicationService
    {
        Task<MeasurementUnitGetResponseModel> CreateAsync(MeasurementUnitCreateRequestModel requestModel);

        Task DeleteAsync(int id);

        Task<MeasurementUnitGetResponseModel> UpdateAsync(int id, MeasurementUnitUpdateRequestModel requestModel);

        Task<MeasurementUnitGetResponseModel> GetAsync(int id);

        Task<PagedResponseModel<MeasurementUnitGetResponseModel>> GetListAsync(MeasurementUnitPagedRequestModel requestModel);
    }
}