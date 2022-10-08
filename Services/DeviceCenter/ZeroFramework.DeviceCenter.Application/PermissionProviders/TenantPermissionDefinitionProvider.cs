using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class TenantPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public TenantPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(TenantPermissions.GroupName, _localizer["Permission:TenantManager"]);

            var productManagement = productGroup.AddPermission(TenantPermissions.Tenants.Default, _localizer["Permission:TenantManager.Tenants"]);

            productManagement.AddChild(TenantPermissions.Tenants.Create, _localizer["Permission:TenantManager.Tenants.Creeate"]);
            productManagement.AddChild(TenantPermissions.Tenants.Edit, _localizer["Permission:TenantManager.Tenants.Edit"]);
            productManagement.AddChild(TenantPermissions.Tenants.Delete, _localizer["Permission:TenantManager.Tenants.Delete"]);

            productManagement.AddChild(TenantPermissions.Tenants.ConnectionString, _localizer["Permission:TenantManager.Tenants.ConnectionString"]);

        }
    }
}