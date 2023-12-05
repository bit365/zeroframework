using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Devices;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    /// <summary>
    /// For more information on enabling Web API for empty projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceGroupsController(IDeviceGroupApplicationService deviceGroupService) : ControllerBase
    {
        private readonly IDeviceGroupApplicationService _deviceGroupService = deviceGroupService;

        // GET: api/<DeviceGroupsController>
        [HttpGet]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Default)]
        public async Task<PagedResponseModel<DeviceGroupGetResponseModel>> GetDeviceGroups([FromQuery] DeviceGroupPagedRequestModel model)
        {
            return await _deviceGroupService.GetListAsync(model);
        }

        [HttpPut("Devices")]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Edit)]
        public async Task<IActionResult> PutDevicesToGroup(int deviceGroupId, [FromBody] long[] deviceIds)
        {
            await _deviceGroupService.AddDevicesToGroup(deviceGroupId, deviceIds);
            return Ok();
        }

        [HttpDelete("Devices")]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Edit)]
        public async Task<IActionResult> DeleteDevicesFromGroup(int deviceGroupId, [FromBody] long[] deviceIds)
        {
            await _deviceGroupService.RemoveDevicesFromGroup(deviceGroupId, deviceIds);
            return Ok();
        }

        // GET api/<DeviceGroupsController>/5
        [HttpGet("{id}")]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Default)]
        public async Task<DeviceGroupGetResponseModel> GetDeviceGroup(int id)
        {
            return await _deviceGroupService.GetAsync(id);
        }

        // POST api/<DeviceGroupsController>
        [HttpPost]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Create)]
        public async Task<DeviceGroupGetResponseModel> PostDeviceGroup([FromBody] DeviceGroupCreateRequestModel value)
        {
            return await _deviceGroupService.CreateAsync(value);
        }

        // PUT api/<DeviceGroupsController>/5
        [HttpPut("{id}")]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Edit)]
        public async Task<DeviceGroupGetResponseModel> PutDeviceGroup(int id, [FromBody] DeviceGroupUpdateRequestModel value)
        {
            value.Id = id;
            return await _deviceGroupService.UpdateAsync(id, value);
        }

        // DELETE api/<DeviceGroupsController>/5
        [HttpDelete("{id}")]
        [Authorize(DeviceGroupPermissions.DeviceGroups.Delete)]
        public async Task DeleteDeviceGroup(int id)
        {
            await _deviceGroupService.DeleteAsync(id);
        }
    }
}