namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class ResourceGroupPermissions
    {
        public const string GroupName = "ResourceGroupManager";

        public static class ResourceGroups
        {
            public const string Default = GroupName + ".ResourceGroups";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
