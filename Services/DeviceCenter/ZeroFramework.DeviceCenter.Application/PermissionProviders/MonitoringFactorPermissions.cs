namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class MonitoringFactorPermissions
    {
        public const string GroupName = "MonitoringFactorManager";

        public static class MonitoringFactors
        {
            public const string Default = GroupName + ".MonitoringFactors";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
