namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class TenantPermissions
    {
        public const string GroupName = "TenantManager";

        public static class Tenants
        {
            public const string Default = GroupName + ".Tenants";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string ConnectionString = Default + ".ConnectionString";
        }
    }
}
