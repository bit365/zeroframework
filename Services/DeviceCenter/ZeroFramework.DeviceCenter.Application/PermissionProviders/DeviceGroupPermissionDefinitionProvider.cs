using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class DeviceGroupPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public DeviceGroupPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var deviceGroupGroup = context.AddGroup(DeviceGroupPermissions.GroupName, _localizer["Permission:DeviceGroupManager"]);

            var deviceGroupManagement = deviceGroupGroup.AddPermission(DeviceGroupPermissions.DeviceGroups.Default, _localizer["Permission:DeviceGroupManager.DeviceGroups"]);
            deviceGroupManagement.AddChild(DeviceGroupPermissions.DeviceGroups.Create, _localizer["Permission:DeviceGroupManager.DeviceGroups.Creeate"]);
            deviceGroupManagement.AddChild(DeviceGroupPermissions.DeviceGroups.Edit, _localizer["Permission:DeviceGroupManager.DeviceGroups.Edit"]);
            deviceGroupManagement.AddChild(DeviceGroupPermissions.DeviceGroups.Delete, _localizer["Permission:DeviceGroupManager.DeviceGroups.Delete"]);
        }
    }
}