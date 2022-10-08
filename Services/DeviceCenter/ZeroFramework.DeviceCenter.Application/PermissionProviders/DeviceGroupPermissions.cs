namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class DeviceGroupPermissions
    {
        public const string GroupName = "DeviceGroupManager";

        public static class DeviceGroups
        {
            public const string Default = GroupName + ".DeviceGroups";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
