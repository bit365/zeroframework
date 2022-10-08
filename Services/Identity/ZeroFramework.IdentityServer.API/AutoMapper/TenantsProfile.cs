using AutoMapper;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Tenants;

namespace ZeroFramework.IdentityServer.API.AutoMapper
{
    public class TenantsProfile : Profile
    {
        public TenantsProfile()
        {
            CreateMap<IdentityTenant, TenantGetResponseModel>();
            CreateMap<TenantCreateRequestModel, IdentityTenant>();
            CreateMap<TenantUpdateRequestModel, IdentityTenant>();

            CreateMap<TenantClaimModel, IdentityTenantClaim>();
            CreateMap<IdentityTenantClaim, TenantClaimModel>();
        }
    }
}
