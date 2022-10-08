using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Commands.Products;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, ProductGetResponseModel>();
            CreateMap<ProductCreateRequestModel, Product>();
            CreateMap<ProductUpdateRequestModel, Product>();
            CreateMap<CreateProductCommand, Product>();

            CreateMap<MeasurementUnit, MeasurementUnitGetResponseModel>();
            CreateMap<MeasurementUnitCreateRequestModel, MeasurementUnit>();
            CreateMap<MeasurementUnitUpdateRequestModel, MeasurementUnit>();
        }
    }
}