using System.Security.Claims;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;

namespace ZeroFramework.DeviceCenter.Application.Services.Permissions
{
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync(string name, Guid? resourceGroupId);

        Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, Guid? resourceGroupId);

        Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, Guid? resourceGroupId);

        Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names, Guid? resourceGroupId);

        Task<bool> IsGrantedAsync(string name, ResourceDescriptor resource);

        Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name, ResourceDescriptor resource);

        Task<bool> IsGrantedAsync(string name);

        Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);

        Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names);

        Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names);
    }
}