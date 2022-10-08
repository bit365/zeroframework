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
        public async Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(Guid productId, long deviceId)
        {
            return await _deviceDataApplication.GetDevicePropertyValues(productId, deviceId);
        }

        [HttpGet("property-history-values")]
        [Authorize(MeasurementPermissions.Measurements.DevicePropertyHistoryValues)]
        public async Task<PageableListResposeModel<DevicePropertyValue>> GetDevicePropertyHistoryValues(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, SortingOrder sorting, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _deviceDataApplication.GetDevicePropertyHistoryValues(productId, deviceId, identifier, startTime, endTime, sorting, skip, pageSize);
        }

        [HttpGet("property-reports")]
        [Authorize(MeasurementPermissions.Measurements.DevicePropertyReports)]
        public async Task<PageableListResposeModel<DevicePropertyReport>> GetDevicePropertyReports(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _deviceDataApplication.GetDevicePropertyReports(productId, deviceId, identifier, startTime, endTime, reportType, skip, pageSize);
        }

        [HttpPut("property-value")]
        [Authorize(MeasurementPermissions.Measurements.SetDevicePropertyValues)]
        public async Task SetDevicePropertyValue([FromQuery] Guid productId, [FromQuery] long deviceId, [FromQuery] string identifier, [FromBody] DevicePropertyValue propertyValue)
        {
            await _deviceDataApplication.SetDevicePropertyValue(productId, deviceId, identifier, propertyValue);
        }

        [HttpPut("property-values")]
        [Authorize(MeasurementPermissions.Measurements.SetDevicePropertyValues)]
        public async Task SetDevicePropertyValues([FromQuery] Guid productId, [FromQuery] long deviceId, [FromBody] params KeyValuePair<string, DevicePropertyValue>[] propertyValues)
        {
            propertyValues = propertyValues ?? throw new ArgumentNullException(nameof(propertyValues));

            foreach (var item in propertyValues)
            {
                await _deviceDataApplication.SetDevicePropertyValue(productId, deviceId, item.Key, item.Value);
            }
        }
    }
}