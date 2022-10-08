namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class DevicePermissions
    {
        public const string GroupName = "DeviceManager";

        public static class Devices
        {
            public const string Default = GroupName + ".Devices";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
