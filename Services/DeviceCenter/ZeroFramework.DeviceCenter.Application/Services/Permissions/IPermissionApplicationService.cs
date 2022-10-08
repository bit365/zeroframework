using ZeroFramework.DeviceCenter.Application.Models.Permissions;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public interface IPermissionApplicationService
    {
        Task<PermissionListResponseModel> GetAsync(string? providerName, string? providerKey, Guid? resourceGroupId);

        Task UpdateAsync(PermissionUpdateRequestModel updateModel);
    }
}