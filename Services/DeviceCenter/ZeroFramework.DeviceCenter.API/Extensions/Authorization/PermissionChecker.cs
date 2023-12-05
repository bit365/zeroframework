using System.Security.Claims;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.API.Extensions.Authorization
{
    public class PermissionChecker(IHttpContextAccessor httpContextAccessor, IPermissionDefinitionManager permissionDefinitionManager, IEnumerable<IPermissionValueProvider> permissionValueProviders, IRepository<ResourceGrouping, Guid> resourceGroupingRepository) : IPermissionChecker
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager = permissionDefinitionManager;

        private readonly IEnumerable<IPermissionValueProvider> _permissionValueProviders = permissionValueProviders;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private readonly IRepository<ResourceGrouping, Guid> _resourceGroupingRepository = resourceGroupingRepository;

        public async Task<bool> IsGrantedAsync(string name, Guid? resourceGroupId) => await IsGrantedAsync(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(), name, resourceGroupId);

        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, Guid? resourceGroupId)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            PermissionDefinition permissionDefinition = _permissionDefinitionManager.Get(name);

            if (!permissionDefinition.IsEnabled)
            {
                return false;
            }

            var isGranted = false;

            foreach (var permissionValueProvider in _permissionValueProviders)
            {
                if (permissionDefinition.AllowedProviders.Any() && !permissionDefinition.AllowedProviders.Contains(permissionValueProvider.Name))
                {
                    continue;
                }

                var result = await permissionValueProvider.CheckAsync(claimsPrincipal, permissionDefinition, resourceGroupId);

                if (result == PermissionGrantResult.Granted)
                {
                    isGranted = true;
                }
                else if (result == PermissionGrantResult.Prohibited)
                {
                    return false;
                }
            }

            return isGranted;
        }

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, Guid? resourceGroupId) => await IsGrantedAsync(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(), names, resourceGroupId);

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names, Guid? resourceGroupId)
        {
            MultiplePermissionGrantResult result = new();

            names ??= Array.Empty<string>();

            List<PermissionDefinition> permissionDefinitions = [];

            foreach (string name in names)
            {
                var permission = _permissionDefinitionManager.Get(name);
                result.Result.Add(name, PermissionGrantResult.Undefined);
                if (permission.IsEnabled)
                {
                    permissionDefinitions.Add(permission);
                }
            }

            foreach (IPermissionValueProvider permissionValueProvider in _permissionValueProviders)
            {
                var pf = permissionDefinitions.Where(x => !x.AllowedProviders.Any() || x.AllowedProviders.Contains(permissionValueProvider.Name)).ToList();

                var multipleResult = await permissionValueProvider.CheckAsync(claimsPrincipal, pf, resourceGroupId);

                foreach (var grantResult in multipleResult.Result.Where(grantResult => result.Result.ContainsKey(grantResult.Key) && result.Result[grantResult.Key] == PermissionGrantResult.Undefined && grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionDefinitions.RemoveAll(x => x.Name == grantResult.Key);
                }

                if (result.AllGranted || result.AllProhibited)
                {
                    break;
                }
            }

            return result;
        }

        public async Task<bool> IsGrantedAsync(string name, ResourceDescriptor resource) => await IsGrantedAsync(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(), name, resource);

        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, ResourceDescriptor resource)
        {
            var resourceGrouping = await _resourceGroupingRepository.FindAsync(e => e.Resource.ResourceId == resource);
            return await IsGrantedAsync(claimsPrincipal, name, resourceGrouping?.ResourceGroupId);
        }

        public async Task<bool> IsGrantedAsync(string name) => await IsGrantedAsync(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(), name);

        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            PermissionDefinition permissionDefinition = _permissionDefinitionManager.Get(name);

            if (!permissionDefinition.IsEnabled)
            {
                return false;
            }

            var isGranted = false;

            foreach (var permissionValueProvider in _permissionValueProviders)
            {
                if (permissionDefinition.AllowedProviders.Any() && !permissionDefinition.AllowedProviders.Contains(permissionValueProvider.Name))
                {
                    continue;
                }

                var result = await permissionValueProvider.CheckAsync(claimsPrincipal, permissionDefinition, Guid.Empty);

                if (result == PermissionGrantResult.Granted)
                {
                    isGranted = true;
                }
                else if (result == PermissionGrantResult.Prohibited)
                {
                    return false;
                }
            }

            return isGranted;
        }

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names) => await IsGrantedAsync(_httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(), names);

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
        {
            MultiplePermissionGrantResult result = new();

            names ??= Array.Empty<string>();

            List<PermissionDefinition> permissionDefinitions = [];

            foreach (string name in names)
            {
                var permission = _permissionDefinitionManager.Get(name);
                result.Result.Add(name, PermissionGrantResult.Undefined);
                if (permission.IsEnabled)
                {
                    permissionDefinitions.Add(permission);
                }
            }

            foreach (IPermissionValueProvider permissionValueProvider in _permissionValueProviders)
            {
                var pfs = permissionDefinitions.Where(x => !x.AllowedProviders.Any() || x.AllowedProviders.Contains(permissionValueProvider.Name)).ToList();

                var multipleResult = await permissionValueProvider.CheckAsync(claimsPrincipal, pfs, Guid.Empty);

                foreach (var grantResult in multipleResult.Result.Where(grantResult => result.Result.ContainsKey(grantResult.Key) && result.Result[grantResult.Key] == PermissionGrantResult.Undefined && grantResult.Value != PermissionGrantResult.Undefined))
                {
                    result.Result[grantResult.Key] = grantResult.Value;
                    permissionDefinitions.RemoveAll(x => x.Name == grantResult.Key);
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