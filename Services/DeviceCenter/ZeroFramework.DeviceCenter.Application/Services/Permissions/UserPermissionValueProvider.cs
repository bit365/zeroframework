using System.Security.Claims;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class UserPermissionValueProvider(IPermissionStore permissionStore) : IPermissionValueProvider
    {
        public const string ProviderName = "User";

        private readonly IPermissionStore _permissionStore = permissionStore;

        public string Name => ProviderName;

        public async Task<PermissionGrantResult> CheckAsync(ClaimsPrincipal principal, PermissionDefinition permission, Guid? resourceGroupId)
        {
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return PermissionGrantResult.Undefined;
            }

            return await _permissionStore.IsGrantedAsync(permission.Name, ProviderName, userId, resourceGroupId) ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined;
        }

        public async Task<MultiplePermissionGrantResult> CheckAsync(ClaimsPrincipal principal, List<PermissionDefinition> permissions, Guid? resourceGroupId)
        {
            var permissionNames = permissions.Select(x => x.Name).ToArray();

            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return new MultiplePermissionGrantResult(permissionNames);
            }

            return await _permissionStore.IsGrantedAsync(permissionNames, ProviderName, userId, resourceGroupId);
        }
    }
}