using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly IDeviceDataApplicationService _deviceDataApplication;

        public MeasurementsController(IDeviceDataApplicationService deviceDataApplication) => _deviceDataApplication = deviceDataApplication;

        [HttpGet("property-values")]
        [Authorize(MeasurementPermissions.Measurements.DevicePropertyValues)]
        public async Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(int productId, long deviceId)
        {
            return await _deviceDataApplication.GetDevicePropertyValues(productId, deviceId);
        }

        [HttpGet("property-history-values")]
        [Authorize(MeasurementPermissions.Measurements.DevicePropertyHistoryValues)]
        public async Task<PageableListResposeModel<DevicePropertyValue>?> GetDevicePropertyHistoryValues(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, SortingOrder sorting, int pageNumber, int pageSize)
        {
            int offset = (pageNumber - 1) * pageSize;

            return await _deviceDataApplication.GetDevicePropertyHistoryValues(productId, deviceId, identifier, startTime, endTime, false, sorting, offset, pageSize);
        }

        [HttpGet("property-reports")]
        [Authorize(MeasurementPermissions.Measurements.DevicePropertyReports)]
        public async Task<PageableListResposeModel<DevicePropertyReport>?> GetDevicePropertyReports(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int pageNumber, int pageSize)
        {
            int offset = (pageNumber - 1) * pageSize;

            return await _deviceDataApplication.GetDevicePropertyReports(productId, deviceId, identifier, startTime, endTime, reportType, offset, pageSize);
        }

        [HttpPut("property-values")]
        [Authorize(MeasurementPermissions.Measurements.SetDevicePropertyValues)]
        public async Task SetDevicePropertyValue([FromQuery] int productId, [FromQuery] long deviceId, [FromBody] IDictionary<string, DevicePropertyValue> values)
        {
            await _deviceDataApplication.SetDevicePropertyValues(productId, deviceId, values);
        }
    }
}