namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class MeasurementPermissions
    {
        public const string GroupName = "MeasurementManager";

        public static class Measurements
        {
            public const string Default = GroupName + ".Measurements";
            public const string DevicePropertyValues = Default + ".DevicePropertyValues";
            public const string DevicePropertyHistoryValues = Default + ".DevicePropertyHistoryValues";
            public const string DevicePropertyReports = Default + ".DevicePropertyReports";
            public const string SetDevicePropertyValues = Default + ".SetDevicePropertyValues";

        }
    }
}
