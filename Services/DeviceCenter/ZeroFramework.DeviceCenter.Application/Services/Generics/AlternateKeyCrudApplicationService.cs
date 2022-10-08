using AutoMapper;
using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public abstract class AlternateKeyCrudApplicationService<TEntity, TKey, TGetResponseModel, TGetListRequestModel, TGetListResponseModel, TCreateRequestModel, TUpdateRequestModel> : ICrudApplicationService<TKey, TGetResponseModel, TGetListRequestModel, TGetListResponseModel, TCreateRequestModel, TUpdateRequestModel> where TEntity : BaseEntity
    {
        protected IRepository<TEntity> Repository { get; }

        private readonly IMapper _mapper;

        public AlternateKeyCrudApplicationService(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TGetResponseModel> CreateAsync(TCreateRequestModel requestModel)
        {
            TEntity entity = _mapper.Map<TEntity>(requestModel);
            await Repository.InsertAsync(entity, true);
            return _mapper.Map<TGetResponseModel>(entity);
        }

        public virtual async Task DeleteAsync(TKey id) => await DeleteByIdAsync(id);

        protected abstract Task DeleteByIdAsync(TKey id);

        public virtual async Task<TGetResponseModel> UpdateAsync(TKey id, TUpdateRequestModel requestModel)
        {
            var entity = await GetEntityByIdAsync(id);
            _mapper.Map(requestModel, entity);
            await Repository.UpdateAsync(entity, true);
            return _mapper.Map<TGetResponseModel>(entity);
        }

        public virtual async Task<TGetResponseModel> GetAsync(TKey id) => _mapper.Map<TGetResponseModel>(await GetEntityByIdAsync(id));

        protected abstract Task<TEntity> GetEntityByIdAsync(TKey id);

        public virtual async Task<PagedResponseModel<TGetListResponseModel>> GetListAsync(TGetListRequestModel requestModel)
        {
            IQueryable<TEntity> query = CreateFilteredQuery(requestModel);
            int totalCount = await Repository.AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, requestModel);
            query = ApplyPaging(query, requestModel);

            var entities = await Repository.AsyncExecuter.ToListAsync(query);

            var entityDtos = _mapper.Map<List<TGetListResponseModel>>(entities);

            return new PagedResponseModel<TGetListResponseModel>(entityDtos, totalCount);
        }

        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetListRequestModel requestModel) => Repository.Query;

        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListRequestModel requestModel)
        {
            var pagedRequestModel = requestModel as PagedRequestModel;

            if (pagedRequestModel is not null && pagedRequestModel.Sorter is not null && pagedRequestModel.Sorter.Any())
            {
                var properties = query.GetType().GetGenericArguments().First().GetProperties();

                IOrderedQueryable<TEntity>? orderedQueryable = null;

                foreach (SortingDescriptor sortingDescriptor in pagedRequestModel.Sorter)
                {
                    string? propertyName = properties.SingleOrDefault(p => string.Equals(p.Name, sortingDescriptor.PropertyName, StringComparison.OrdinalIgnoreCase))?.Name;

                    if (propertyName is null)
                    {
                        throw new KeyNotFoundException(sortingDescriptor.PropertyName);
                    }

                    if (sortingDescriptor.SortDirection == SortingOrder.Ascending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderBy(propertyName) : orderedQueryable.ThenBy(propertyName);
                    }
                    else if (sortingDescriptor.SortDirection == SortingOrder.Descending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderByDescending(propertyName) : orderedQueryable.ThenByDescending(propertyName);
                    }
                }

                return orderedQueryable ?? query;
            }

            string firstPropertyName = typeof(TEntity).GetProperties().First().Name;
            return query.OrderByDescending(firstPropertyName);
        }

        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListRequestModel requestModel)
        {
            if (requestModel is PagedRequestModel model)
            {
                return query.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize);
            }

            return query;
        }
    }
}