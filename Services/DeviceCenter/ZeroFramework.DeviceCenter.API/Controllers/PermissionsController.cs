using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Permissions;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(IPermissionApplicationService permissionApplicationService) : ControllerBase
    {
        private readonly IPermissionApplicationService _permissionApplicationService = permissionApplicationService;

        [HttpPut]
        [Authorize(PermissionPermissions.Permissions.Edit)]
        public virtual Task UpdateAsync(PermissionUpdateRequestModel updateModel)
        {
            return _permissionApplicationService.UpdateAsync(updateModel);
        }

        [HttpGet]
        [Authorize(PermissionPermissions.Permissions.Get)]
        public virtual Task<PermissionListResponseModel> GetAsync(string? providerName, string? providerKey, Guid? resourceGroupId)
        {
            return _permissionApplicationService.GetAsync(providerName, providerKey, resourceGroupId);
        }
    }
}
