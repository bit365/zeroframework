using ZeroFramework.DeviceCenter.Application.Models.Permissions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class PermissionDataSeedProvider(IPermissionApplicationService permissionService, IPermissionDefinitionManager permissionDefinitionManager, ICurrentTenant currentTenant, IRepository<ResourceGroup, Guid> resourceGroupRepository) : IDataSeedProvider
    {
        private readonly IPermissionApplicationService _permissionService = permissionService;

        private readonly IPermissionDefinitionManager _permissionDefinitionManager = permissionDefinitionManager;

        private readonly ICurrentTenant _currentTenant = currentTenant;

        private readonly IRepository<ResourceGroup, Guid> _resourceGroupRepository = resourceGroupRepository;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            ResourceGroup? resourceGroup = await _resourceGroupRepository.FindAsync(e => e.Name == ResourceGroup.DefaultGroup);

            if (resourceGroup is null)
            {
                resourceGroup = await _resourceGroupRepository.InsertAsync(new ResourceGroup { Name = ResourceGroup.DefaultGroup, DisplayName = ResourceGroup.DefaultGroup }, true);
            }

            var permissionNames = _permissionDefinitionManager.GetPermissions().Where(p => !p.AllowedProviders.Any() || p.AllowedProviders.Contains(RolePermissionValueProvider.ProviderName)).Select(p => p.Name).ToArray();

            //permissionNames = permissionNames.Where(e => !e.StartsWith("ResourceGroupManager.ResourceGroups")).ToArray();

            PermissionUpdateRequestModel updateModel = new()
            {
                ProviderInfos = new List<PermissionProviderInfoModel>
                {
                    new() {ProviderName = UserPermissionValueProvider.ProviderName, ProviderKey = "1"}
                },

                PermissionGrantInfos = Array.ConvertAll(permissionNames, pn => new PermissionGrantInfoModel { Name = pn, IsGranted = true }),
                ResourceGroupId = resourceGroup.Id
            };

            await _permissionService.UpdateAsync(updateModel);

            using (_currentTenant.Change(Guid.Parse("5f6f2110-58b6-4cf9-b416-85820ba12c01")))
            {
                resourceGroup = await _resourceGroupRepository.FindAsync(e => e.Name == ResourceGroup.DefaultGroup);

                if (resourceGroup is null)
                {
                    resourceGroup = await _resourceGroupRepository.InsertAsync(new ResourceGroup { Name = ResourceGroup.DefaultGroup, DisplayName = ResourceGroup.DefaultGroup }, true);
                }

                updateModel.ProviderInfos = new List<PermissionProviderInfoModel>
                {
                    new() {ProviderName = RolePermissionValueProvider.ProviderName, ProviderKey = "role1@tenant1"}
                };
                updateModel.ResourceGroupId = resourceGroup.Id;

                await _permissionService.UpdateAsync(updateModel);
            }
        }
    }
}