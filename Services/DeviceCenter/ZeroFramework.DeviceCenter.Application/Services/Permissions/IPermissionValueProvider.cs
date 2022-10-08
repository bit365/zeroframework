using System.Security.Claims;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public interface IPermissionValueProvider
    {
        string Name { get; }

        Task<PermissionGrantResult> CheckAsync(ClaimsPrincipal principal, PermissionDefinition permission, Guid? resourceGroupId);

        Task<MultiplePermissionGrantResult> CheckAsync(ClaimsPrincipal principal, List<PermissionDefinition> permissions, Guid? resourceGroupId);
    }
}
