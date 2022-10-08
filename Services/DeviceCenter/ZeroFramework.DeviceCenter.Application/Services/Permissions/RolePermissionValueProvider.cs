using System.Security.Claims;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class RolePermissionValueProvider : IPermissionValueProvider
    {
        public const string ProviderName = "Role";

        private readonly IPermissionStore _permissionStore;

        public string Name => ProviderName;

        public RolePermissionValueProvider(IPermissionStore permissionStore)
        {
            _permissionStore = permissionStore;
        }

        public async Task<PermissionGrantResult> CheckAsync(ClaimsPrincipal principal, PermissionDefinition permission, Guid? resourceGroupId)
        {
            var roles = principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles is null || !roles.Any())
            {
                return PermissionGrantResult.Undefined;
            }

            foreach (var role in roles)
            {
                if (await _permissionStore.IsGrantedAsync(permission.Name, ProviderName, role, resourceGroupId))
                {
                    return PermissionGrantResult.Granted;
                }
            }

            return PermissionGrantResult.Undefined;
        }

        public async Task<MultiplePermissionGrantResult> CheckAsync(ClaimsPrincipal principal, List<PermissionDefinition> permissions, Guid? resourceGroupId)
        {
            var permissionNames = permissions.Select(x => x.Name).ToList();
            var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

            var roles = principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles is null || !roles.Any())
            {
                return result;
            }

            foreach (var role in roles)
            {
                var multipleResult = await _permissionStore.IsGrantedAsync(permissionNames.ToArray(), ProviderName, role, resourceGroupId);

                foreach (var grantResult in multipleResult.Result.Where(grantResult => result.Result.ContainsKey(grantResult.Key) && result.Result[grantResult.Key] == PermissionGrantResult.Undefined && grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionNames.RemoveAll(x => x == grantResult.Key);
                }

                if (result.AllGranted || result.AllProhibited)
                {
                    break;
                }
            }

            return result;
        }
    }
}
