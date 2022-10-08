using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class PermissionPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public PermissionPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(PermissionPermissions.GroupName, _localizer["Permission:PermissionManager"]);

            var productManagement = productGroup.AddPermission(PermissionPermissions.Permissions.Default, _localizer["Permission:PermissionManager.Permissions"]);

            productManagement.AddChild(PermissionPermissions.Permissions.Get, _localizer["Permission:PermissionManager.Permissions.Get"]);
            productManagement.AddChild(PermissionPermissions.Permissions.Edit, _localizer["Permission:PermissionManager.Permissions.Edit"]);
        }
    }
}