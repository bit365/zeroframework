using AutoMapper;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Users;

namespace ZeroFramework.IdentityServer.API.AutoMapper
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<ApplicationUser, UserGetResponseModel>();
            CreateMap<UserCreateRequestModel, ApplicationUser>();
            CreateMap<UserUpdateRequestModel, ApplicationUser>();
        }
    }
}
