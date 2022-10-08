using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.ResourceGroups;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class ResourceGroupsProfile : Profile
    {
        public ResourceGroupsProfile()
        {
            CreateMap<ResourceGroup, ResourceGroupGetResponseModel>();
            CreateMap<ResourceGroupCreateRequestModel, ResourceGroup>();
            CreateMap<ResourceGroupUpdateRequestModel, ResourceGroup>();
        }
    }
}