using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Queries.Monitoring;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringFactorsController : ControllerBase
    {
        private readonly ICrudApplicationService<int, MonitoringFactorGetResponseModel, MonitoringFactorPagedRequestModel, MonitoringFactorGetResponseModel, MonitoringFactorCreateRequestModel, MonitoringFactorUpdateRequestModel> _crudApplicationService;

        private readonly IMonitoringFactorQueries _monitoringFactorQueries;

        public MonitoringFactorsController(IMonitoringFactorQueries monitoringFactorQueries, ICrudApplicationService<int, MonitoringFactorGetResponseModel, MonitoringFactorPagedRequestModel, MonitoringFactorGetResponseModel, MonitoringFactorCreateRequestModel, MonitoringFactorUpdateRequestModel> crudApplicationService)
        {
            _monitoringFactorQueries = monitoringFactorQueries;
            _crudApplicationService = crudApplicationService;
        }

        [HttpGet]
        [Authorize(MonitoringFactorPermissions.MonitoringFactors.Default)]
        public async Task<PagedResponseModel<MonitoringFactorGetResponseModel>> GetMonitoringFactors([FromQuery] MonitoringFactorPagedRequestModel model)
        {
            return await _monitoringFactorQueries.GetMonitoringFactorsAsync(model);
        }

        [HttpGet("{id:int}")]
        [Authorize(MonitoringFactorPermissions.MonitoringFactors.Default)]
        public async Task<MonitoringFactorGetResponseModel> GetMonitoringFactor(int id)
        {
            return await _monitoringFactorQueries.GetMonitoringFactorAsync(id);
        }

        [HttpPost]
        [Authorize(MonitoringFactorPermissions.MonitoringFactors.Create)]
        public async Task<MonitoringFactorGetResponseModel> PostMonitoringFactor([FromBody] MonitoringFactorCreateRequestModel model)
        {
            return await _crudApplicationService.CreateAsync(model);
        }

        [HttpPut("{id:int}")]
        [Authorize(MonitoringFactorPermissions.MonitoringFactors.Edit)]
        public async Task<MonitoringFactorGetResponseModel> PutMonitoringFactor(int id, [FromBody] MonitoringFactorUpdateRequestModel model)
        {
            model.Id = id;
            return await _crudApplicationService.UpdateAsync(id, model);
        }

        [HttpDelete("{id:int}")]
        [Authorize(MonitoringFactorPermissions.MonitoringFactors.Delete)]
        public async Task DeleteMonitoringFactor(int id)
        {
            await _crudApplicationService.DeleteAsync(id);
        }
    }
}
