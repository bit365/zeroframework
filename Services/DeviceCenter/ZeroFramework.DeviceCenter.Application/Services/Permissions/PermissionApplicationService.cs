using Microsoft.Extensions.Caching.Distributed;
using ZeroFramework.DeviceCenter.Application.Models.Permissions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class PermissionApplicationService(IPermissionDefinitionManager permissionDefinitionManager, IPermissionGrantRepository permissionGrantRepository, IDistributedCache distributedCache) : IPermissionApplicationService
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager = permissionDefinitionManager;

        private readonly IPermissionGrantRepository _permissionGrantRepository = permissionGrantRepository;

        private const string CacheKeyFormat = "pn:{0},pk:{1},on:{2},rg:{3}";

        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<PermissionListResponseModel> GetAsync(string? providerName, string? providerKey, Guid? resourceGroupId)
        {
            var result = new PermissionListResponseModel { EntityDisplayName = providerKey, Groups = [] };

            foreach (var group in _permissionDefinitionManager.GetGroups())
            {
                PermissionGroupModel permissionGroupModel = new()
                {
                    DisplayName = group.DisplayName,
                    Name = group.Name,
                    Permissions = []
                };

                foreach (PermissionDefinition? permission in group.GetPermissionsWithChildren())
                {
                    if (permission.IsEnabled && (!permission.AllowedProviders.Any() || (providerName is not null && permission.AllowedProviders.Contains(providerName))))
                    {
                        PermissionGrantModel permissionGrantModel = new()
                        {
                            Name = permission.Name,
                            DisplayName = permission.DisplayName,
                            ParentName = permission.Parent?.Name ?? string.Empty,
                            AllowedProviders = permission.AllowedProviders
                        };

                        if (permission.AllowedProviders.Any() && providerName is not null && !permission.AllowedProviders.Contains(providerName))
                        {
                            throw new ApplicationException($"The permission named {permission.Name} has not compatible with the provider named {providerName}");
                        }

                        if (!permission.IsEnabled)
                        {
                            throw new ApplicationException($"The permission named {permission.Name} is disabled");
                        }

                        PermissionGrant? permissionGrant = null;

                        if (providerName is not null && providerKey is not null)
                        {
                            permissionGrant = await _permissionGrantRepository.FindAsync(permission.Name, providerName, providerKey, resourceGroupId);
                        }

                        permissionGrantModel.IsGranted = permissionGrant != null;

                        permissionGroupModel.Permissions.Add(permissionGrantModel);
                    }
                }

                if (permissionGroupModel.Permissions.Any())
                {
                    result.Groups.Add(permissionGroupModel);
                }
            }

            return result;
        }

        public async Task UpdateAsync(PermissionUpdateRequestModel updateModel)
        {

            foreach (PermissionProviderInfoModel providerInfo in updateModel.ProviderInfos)
            {
                foreach (PermissionGrantInfoModel grantInfo in updateModel.PermissionGrantInfos)
                {
                    var permission = _permissionDefinitionManager.Get(grantInfo.Name);

                    if (permission.AllowedProviders.Any() && !permission.AllowedProviders.Contains(providerInfo.ProviderName))
                    {
                        throw new ApplicationException($"The permission named {permission.Name} has not compatible with the provider named {providerInfo.ProviderName}");
                    }

                    if (!permission.IsEnabled)
                    {
                        throw new ApplicationException($"The permission named {permission.Name} is disabled");
                    }

                    PermissionGrant? permissionGrant = await _permissionGrantRepository.FindAsync(grantInfo.Name, providerInfo.ProviderName, providerInfo.ProviderKey, updateModel.ResourceGroupId);


                    if (grantInfo.IsGranted && permissionGrant is null)
                    {
                        await _permissionGrantRepository.InsertAsync(new PermissionGrant { OperationName = grantInfo.Name, ProviderName = providerInfo.ProviderName, ProviderKey = providerInfo.ProviderKey, ResourceGroupId = updateModel.ResourceGroupId }, true);
                    }

                    if (!grantInfo.IsGranted && permissionGrant is not null)
                    {
                        await _permissionGrantRepository.DeleteAsync(permissionGrant);
                    }

                    var cacheKey = string.Format(CacheKeyFormat, providerInfo.ProviderName, providerInfo.ProviderKey, grantInfo.Name, updateModel.ResourceGroupId);

                    await _distributedCache.SetStringAsync(cacheKey, grantInfo.IsGranted.ToString());
                }
            }

            await _permissionGrantRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}