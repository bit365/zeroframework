using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.API.Extensions.Authorization
{
    public class CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionDefinitionManager permissionDefinitionManager) : DefaultAuthorizationPolicyProvider(options)
    {
        private readonly IPermissionDefinitionManager _permissionDefinitionManager = permissionDefinitionManager;

        public async override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
            {
                return policy;
            }

            var permission = _permissionDefinitionManager.GetOrNull(policyName);

            if (permission is not null)
            {
                var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
                policyBuilder.Requirements.Add(new OperationAuthorizationRequirement { Name = policyName });
                return policyBuilder.Build();
            }

            return null;
        }
    }
}