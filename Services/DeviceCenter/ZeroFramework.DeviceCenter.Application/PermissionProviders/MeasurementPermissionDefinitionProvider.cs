using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public class MeasurementPermissionDefinitionProvider : IPermissionDefinitionProvider
    {
        private readonly IStringLocalizer _localizer;

        public MeasurementPermissionDefinitionProvider(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("Permissions.MyPermissions", Assembly.GetExecutingAssembly().FullName ?? string.Empty);
        }

        public void Define(PermissionDefinitionContext context)
        {
            var measurementGroup = context.AddGroup(MeasurementPermissions.GroupName, _localizer["Permission:MeasurementManager"]);

            var measurementManagement = measurementGroup.AddPermission(MeasurementPermissions.Measurements.Default, _localizer["Permission:MeasurementManager.Measurements"]);
            measurementManagement.AddChild(MeasurementPermissions.Measurements.DevicePropertyValues, _localizer["Permission:MeasurementManager.Measurements.DevicePropertyValues"]);
            measurementManagement.AddChild(MeasurementPermissions.Measurements.DevicePropertyHistoryValues, _localizer["Permission:MeasurementManager.Measurements.DevicePropertyHistoryValues"]);
            measurementManagement.AddChild(MeasurementPermissions.Measurements.DevicePropertyReports, _localizer["Permission:MeasurementManager.Measurements.DevicePropertyReports"]);
            measurementManagement.AddChild(MeasurementPermissions.Measurements.SetDevicePropertyValues, _localizer["Permission:MeasurementManager.Measurements.SetDevicePropertyValues"]);
        }
    }
}