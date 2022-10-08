using AutoMapper;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Roles;

namespace ZeroFramework.IdentityServer.API.AutoMapper
{
    public class RolesProfile : Profile
    {
        public RolesProfile()
        {
            CreateMap<ApplicationRole, RoleGetResponseModel>();
            CreateMap<RoleCreateRequestModel, ApplicationRole>();
            CreateMap<RoleUpdateRequestModel, ApplicationRole>();
        }
    }
}
