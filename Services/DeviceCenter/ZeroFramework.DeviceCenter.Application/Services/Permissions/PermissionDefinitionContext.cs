﻿namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public class PermissionDefinitionContext
    {
        public IServiceProvider ServiceProvider { get; }

        internal Dictionary<string, PermissionGroupDefinition> Groups { get; }

        internal PermissionDefinitionContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }

        public virtual PermissionGroupDefinition AddGroup(string name, string? displayName = null)
        {
            if (Groups.ContainsKey(name))
            {
                throw new InvalidOperationException($"There is already an existing permission group with name: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }

        public virtual PermissionGroupDefinition GetGroup(string name)
        {
            PermissionGroupDefinition? group = GetGroupOrNull(name);

            if (group is null)
            {
                throw new InvalidOperationException($"Could not find a permission definition group with the given name: {name}");
            }

            return group;
        }

        public virtual PermissionGroupDefinition? GetGroupOrNull(string name)
        {
            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        public virtual void RemoveGroup(string name)
        {
            if (!Groups.ContainsKey(name))
            {
                throw new InvalidOperationException($"Not found permission group with name: {name}");
            }

            Groups.Remove(name);
        }

        public virtual PermissionDefinition? GetPermissionOrNull(string name)
        {
            foreach (var groupDefinition in Groups.Values)
            {
                var permissionDefinition = groupDefinition.GetPermissionOrNull(name);

                if (permissionDefinition != null)
                {
                    return permissionDefinition;
                }
            }

            return null;
        }
    }
}
