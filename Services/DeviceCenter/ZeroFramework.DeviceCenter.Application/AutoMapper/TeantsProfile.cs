using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Tenants;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class TeantsProfile : Profile
    {
        public TeantsProfile()
        {
            CreateMap<Tenant, TenantGetResponseModel>();
            CreateMap<TenantCreateOrUpdateRequestModel, Tenant>();
        }
    }
}
