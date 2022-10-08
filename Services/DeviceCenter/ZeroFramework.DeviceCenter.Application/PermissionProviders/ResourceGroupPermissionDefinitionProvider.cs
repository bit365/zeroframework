using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class ResourceGroupPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public ResourceGroupPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(ResourceGroupPermissions.GroupName, _localizer["Permission:ResourceGroupManager"]);

            var productManagement = productGroup.AddPermission(ResourceGroupPermissions.ResourceGroups.Default, _localizer["Permission:ResourceGroupManager.ResourceGroups"]);

            productManagement.AddChild(ResourceGroupPermissions.ResourceGroups.Create, _localizer["Permission:ResourceGroupManager.ResourceGroups.Creeate"]);
            productManagement.AddChild(ResourceGroupPermissions.ResourceGroups.Edit, _localizer["Permission:ResourceGroupManager.ResourceGroups.Edit"]);
            productManagement.AddChild(ResourceGroupPermissions.ResourceGroups.Delete, _localizer["Permission:ResourceGroupManager.ResourceGroups.Delete"]);
        }
    }
}