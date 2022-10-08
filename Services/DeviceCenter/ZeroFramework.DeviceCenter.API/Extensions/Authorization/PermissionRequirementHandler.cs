using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;

namespace ZeroFramework.DeviceCenter.API.Extensions.Authorization
{
    public class PermissionRequirementHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;

        public PermissionRequirementHandler(IPermissionChecker permissionChecker) => _permissionChecker = permissionChecker;

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.Name))
            {
                context.Succeed(requirement);
            }
        }
    }

    public class ResourcePermissionRequirementHandler : AuthorizationHandler<OperationAuthorizationRequirement, ResourceDescriptor>
    {
        private readonly IPermissionChecker _permissionChecker;

        public ResourcePermissionRequirementHandler(IPermissionChecker permissionChecker) => _permissionChecker = permissionChecker;

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ResourceDescriptor resource)
        {
            if (await _permissionChecker.IsGrantedAsync(context.User, requirement.Name, resource))
            {
                context.Succeed(requirement);
            }
        }
    }
}