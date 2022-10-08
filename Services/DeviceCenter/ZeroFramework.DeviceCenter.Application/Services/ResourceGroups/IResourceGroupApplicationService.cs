using ZeroFramework.DeviceCenter.Application.Models.ResourceGroups;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.ResourceGroups
{
    public interface IResourceGroupApplicationService : ICrudApplicationService<Guid, ResourceGroupGetResponseModel, ResourceGroupPagedRequestModel, ResourceGroupGetResponseModel, ResourceGroupCreateRequestModel, ResourceGroupUpdateRequestModel>
    {
        Task<ResourceGroupGetResponseModel> GetOrAddDefaultGroupAsync();
    }
}