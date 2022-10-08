using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Ordering;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class BuyersProfile : Profile
    {
        public BuyersProfile()
        {
            CreateMap<OrderCreateRequestModel, Order>();
            CreateMap<IEnumerable<Order>, Order>();
        }
    }
}
