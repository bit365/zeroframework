using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Products
{
    public class ProductApplicationService(IRepository<Product, int> repository, IMapper mapper) : CrudApplicationService<Product, int, ProductGetResponseModel, ProductPagedRequestModel, ProductGetResponseModel, ProductCreateRequestModel, ProductUpdateRequestModel>(repository, mapper), IProductApplicationService
    {
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