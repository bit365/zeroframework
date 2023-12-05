using AutoMapper;
using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public class CrudApplicationService<TEntity, TKey, TGetResponseModel, TGetListRequestModel, TGetListResponseModel, TCreateRequestModel, TUpdateRequestModel>(IRepository<TEntity, TKey> repository, IMapper mapper) : AlternateKeyCrudApplicationService<TEntity, TKey, TGetResponseModel, TGetListRequestModel, TGetListResponseModel, TCreateRequestModel, TUpdateRequestModel>(repository, mapper) where TEntity : BaseEntity<TKey>
    {
        protected new IRepository<TEntity, TKey> Repository { get; } = repository;

        protected async override Task DeleteByIdAsync(TKey id) => await Repository.DeleteAsync(id, true);

        protected async override Task<TEntity> GetEntityByIdAsync(TKey id) => await Repository.GetAsync(id);
    }
}