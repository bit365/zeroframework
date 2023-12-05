using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class MonitoringFactorPermissionDefinitionProvider(IStringLocalizerFactory factory) : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(MonitoringFactorPermissions.GroupName, _localizer["Permission:MonitoringFactorManager"]);

            var productManagement = productGroup.AddPermission(MonitoringFactorPermissions.MonitoringFactors.Default, _localizer["Permission:MonitoringFactorManager.MonitoringFactors"]);

            productManagement.AddChild(MonitoringFactorPermissions.MonitoringFactors.Create, _localizer["Permission:MonitoringFactorManager.MonitoringFactors.Creeate"]);
            productManagement.AddChild(MonitoringFactorPermissions.MonitoringFactors.Edit, _localizer["Permission:MonitoringFactorManager.MonitoringFactors.Edit"]);
            productManagement.AddChild(MonitoringFactorPermissions.MonitoringFactors.Delete, _localizer["Permission:MonitoringFactorManager.MonitoringFactors.Delete"]);
        }
    }
}