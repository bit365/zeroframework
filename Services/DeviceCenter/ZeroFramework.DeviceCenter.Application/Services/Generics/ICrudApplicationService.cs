namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public interface ICrudApplicationService<TKey, TGetResponseModel, TGetListRequestModel, TGetListResponseModel, TCreateRequestModel, TUpdateRequestModel>
    {
        Task<TGetResponseModel> CreateAsync(TCreateRequestModel requestModel);

        Task DeleteAsync(TKey id);

        Task<TGetResponseModel> UpdateAsync(TKey id, TUpdateRequestModel requestModel);

        Task<TGetResponseModel> GetAsync(TKey id);

        Task<PagedResponseModel<TGetListResponseModel>> GetListAsync(TGetListRequestModel requestModel);
    }
}