namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class PermissionPermissions
    {
        public const string GroupName = "PermissionManager";

        public static class Permissions
        {
            public const string Default = GroupName + ".Permissions";
            public const string Get = Default + ".Get";
            public const string Edit = Default + ".Edit";
        }
    }
}
