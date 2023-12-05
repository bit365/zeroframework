using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class DevicePermissionDefinitionProvider(IStringLocalizerFactory factory) : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);

        public void Define(PermissionDefinitionContext context)
        {
            var deviceGroup = context.AddGroup(DevicePermissions.GroupName, _localizer["Permission:DeviceManager"]);

            var deviceManagement = deviceGroup.AddPermission(DevicePermissions.Devices.Default, _localizer["Permission:DeviceManager.Devices"]);
            deviceManagement.AddChild(DevicePermissions.Devices.Create, _localizer["Permission:DeviceManager.Devices.Creeate"]);
            deviceManagement.AddChild(DevicePermissions.Devices.Edit, _localizer["Permission:DeviceManager.Devices.Edit"]);
            deviceManagement.AddChild(DevicePermissions.Devices.Delete, _localizer["Permission:DeviceManager.Devices.Delete"]);
        }
    }
}