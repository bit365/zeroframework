using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class ProductPermissionDefinitionProvider(IStringLocalizerFactory factory) : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);

        public void Define(PermissionDefinitionContext context)
        {
            var productGroup = context.AddGroup(ProductPermissions.GroupName, _localizer["Permission:ProductManager"]);

            var productManagement = productGroup.AddPermission(ProductPermissions.Products.Default, _localizer["Permission:ProductManager.Products"]);
            productManagement.AddChild(ProductPermissions.Products.Create, _localizer["Permission:ProductManager.Products.Creeate"]);
            productManagement.AddChild(ProductPermissions.Products.Edit, _localizer["Permission:ProductManager.Products.Edit"]);
            productManagement.AddChild(ProductPermissions.Products.Delete, _localizer["Permission:ProductManager.Products.Delete"]);

            var measurementUnitManagement = productGroup.AddPermission(ProductPermissions.MeasurementUnits.Default, _localizer["Permission:ProductManager.MeasurementUnits"]);
            measurementUnitManagement.AddChild(ProductPermissions.MeasurementUnits.Create, _localizer["Permission:ProductManager.MeasurementUnits.Creeate"]);
            measurementUnitManagement.AddChild(ProductPermissions.MeasurementUnits.Edit, _localizer["Permission:ProductManager.MeasurementUnits.Edit"]);
            measurementUnitManagement.AddChild(ProductPermissions.MeasurementUnits.Delete, _localizer["Permission:ProductManager.MeasurementUnits.Delete"]);
        }
    }
}