using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class ProjectPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public ProjectPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(ProjectPermissions.GroupName, _localizer["Permission:ProjectManager"]);

            var productManagement = productGroup.AddPermission(ProjectPermissions.Projects.Default, _localizer["Permission:ProjectManager.Projects"]);

            productManagement.AddChild(ProjectPermissions.Projects.Create, _localizer["Permission:ProjectManager.Projects.Creeate"]);
            productManagement.AddChild(ProjectPermissions.Projects.Edit, _localizer["Permission:ProjectManager.Projects.Edit"]);
            productManagement.AddChild(ProjectPermissions.Projects.Delete, _localizer["Permission:ProjectManager.Projects.Delete"]);
        }
    }
}