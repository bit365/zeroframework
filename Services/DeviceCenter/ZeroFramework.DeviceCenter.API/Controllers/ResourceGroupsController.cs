using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.ResourceGroups;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.ResourceGroups;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceGroupsController : ControllerBase
    {
        private readonly IResourceGroupApplicationService _resourceGroupApplicationService;

        public ResourceGroupsController(IResourceGroupApplicationService resourceGroupApplicationService)
        {
            _resourceGroupApplicationService = resourceGroupApplicationService;
        }

        [HttpGet]
        [Authorize(ResourceGroupPermissions.ResourceGroups.Default)]
        public async Task<PagedResponseModel<ResourceGroupGetResponseModel>> GetResourceGroups([FromQuery] ResourceGroupPagedRequestModel model)
        {
            return await _resourceGroupApplicationService.GetListAsync(model);
        }

        [HttpGet("{id:guid}")]
        [Authorize(ResourceGroupPermissions.ResourceGroups.Default)]
        public async Task<ResourceGroupGetResponseModel> GetResourceGroup(Guid id)
        {
            return await _resourceGroupApplicationService.GetAsync(id);
        }

        [HttpPost]
        [Authorize(ResourceGroupPermissions.ResourceGroups.Create)]
        public async Task<ResourceGroupGetResponseModel> PostResourceGroup([FromBody] ResourceGroupCreateRequestModel model)
        {
            return await _resourceGroupApplicationService.CreateAsync(model);
        }

        [HttpPut("{id:guid}")]
        [Authorize(ResourceGroupPermissions.ResourceGroups.Edit)]
        public async Task<ResourceGroupGetResponseModel> PutResourceGroup(Guid id, [FromBody] ResourceGroupUpdateRequestModel model)
        {
            model.Id = id;
            return await _resourceGroupApplicationService.UpdateAsync(id, model);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(ResourceGroupPermissions.ResourceGroups.Delete)]
        public async Task DeleteResourceGroup(Guid id)
        {
            await _resourceGroupApplicationService.DeleteAsync(id);
        }
    }
}
