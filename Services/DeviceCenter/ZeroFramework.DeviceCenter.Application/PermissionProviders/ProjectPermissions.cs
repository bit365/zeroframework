namespace ZeroFramework.DeviceCenter.Application.PermissionProviders
{
    public static class ProjectPermissions
    {
        public const string GroupName = "ProjectManager";

        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
