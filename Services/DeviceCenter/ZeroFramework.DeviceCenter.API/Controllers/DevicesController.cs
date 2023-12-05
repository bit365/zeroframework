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
    public class DevicesController(IDeviceApplicationService deviceService) : ControllerBase
    {
        private readonly IDeviceApplicationService _deviceService = deviceService;

        // GET: api/<DevicesController>
        [HttpGet]
        [Authorize(DevicePermissions.Devices.Default)]
        public async Task<PagedResponseModel<DeviceGetResponseModel>> GetDevices([FromQuery] DevicePagedRequestModel model)
        {
            return await _deviceService.GetListAsync(model);
        }

        // GET api/<DevicesController>/5
        [HttpGet("{id}")]
        [Authorize(DevicePermissions.Devices.Default)]
        public async Task<DeviceGetResponseModel> GetDevice(long id)
        {
            return await _deviceService.GetAsync(id);
        }

        // POST api/<DevicesController>
        [HttpPost]
        [Authorize(DevicePermissions.Devices.Create)]
        public async Task<DeviceGetResponseModel> PostDevice([FromBody] DeviceCreateRequestModel value)
        {
            return await _deviceService.CreateAsync(value);
        }

        // PUT api/<DevicesController>/5
        [HttpPut("{id}")]
        [Authorize(DevicePermissions.Devices.Edit)]
        public async Task<DeviceGetResponseModel> PutDevice(long id, [FromBody] DeviceUpdateRequestModel value)
        {
            value.Id = id;
            return await _deviceService.UpdateAsync(id, value);
        }

        // DELETE api/<DevicesController>/5
        [HttpDelete("{id}")]
        [Authorize(DevicePermissions.Devices.Delete)]
        public async Task DeleteDevice(long id)
        {
            await _deviceService.DeleteAsync(id);
        }

        [HttpGet("statistic")]
        [Authorize(DevicePermissions.Devices.Default)]
        public async Task<DeviceStatisticGetResponseModel> GetStatistic()
        {
            return await _deviceService.GetStatistics();
        }
    }
}