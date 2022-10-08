using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public class ProductApplicationService : CrudApplicationService<Product, Guid, ProductGetResponseModel, ProductPagedRequestModel, ProductGetResponseModel, ProductCreateRequestModel, ProductUpdateRequestModel>, IProductApplicationService
    {
        public ProductApplicationService(IRepository<Product, Guid> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override IQueryable<Product> CreateFilteredQuery(ProductPagedRequestModel requestModel)
        {
            if (requestModel.Keyword is not null && !string.IsNullOrWhiteSpace(requestModel.Keyword))
            {
                return Repository.Query.Where(e => e.Name.Contains(requestModel.Keyword));
            }

            return base.CreateFilteredQuery(requestModel);
        }
    }
}