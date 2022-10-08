using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Tenants;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Tenants;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    /// <summary>
    /// For more information on enabling Web API for empty projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController, ApiExplorerSettings(IgnoreApi = true)]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantApplicationService _tenantService;

        public TenantsController(ITenantApplicationService tenantService)
        {
            _tenantService = tenantService;
        }

        // GET: api/<TenantsController>
        [HttpGet]
        [Authorize(TenantPermissions.Tenants.Default)]
        public async Task<PagedResponseModel<TenantGetResponseModel>> Get([FromQuery] PagedRequestModel model)
        {
            return await _tenantService.GetListAsync(model);
        }

        // GET api/<TenantsController>/5
        [HttpGet("{id}")]
        [Authorize(TenantPermissions.Tenants.Default)]
        public async Task<TenantGetResponseModel> Get(Guid id)
        {
            return await _tenantService.GetAsync(id);
        }

        // POST api/<TenantsController>
        [HttpPost]
        [Authorize(TenantPermissions.Tenants.Create)]
        public async Task<TenantGetResponseModel> Post([FromBody] TenantCreateOrUpdateRequestModel value)
        {
            return await _tenantService.CreateAsync(value);
        }

        // PUT api/<TenantsController>/5
        [HttpPut("{id}")]
        [Authorize(TenantPermissions.Tenants.Edit)]
        public async Task<TenantGetResponseModel> Put(Guid id, [FromBody] TenantCreateOrUpdateRequestModel value)
        {
            value.Id = id;
            return await _tenantService.UpdateAsync(id, value);
        }

        // DELETE api/<TenantsController>/5
        [HttpDelete("{id}")]
        [Authorize(TenantPermissions.Tenants.Delete)]
        public async Task Delete(Guid id)
        {
            await _tenantService.DeleteAsync(id);
        }

        [HttpGet("{id}/default-connection-string")]
        [Authorize(TenantPermissions.Tenants.ConnectionString)]
        public async Task<string?> GetDefaultConnectionStringAsync(Guid id)
        {
            return await _tenantService.GetDefaultConnectionStringAsync(id);
        }

        [HttpPut("{id}/default-connection-string")]
        [Authorize(TenantPermissions.Tenants.ConnectionString)]
        public async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
        {
            await _tenantService.UpdateDefaultConnectionStringAsync(id, defaultConnectionString);
        }

        [HttpDelete("{id}/default-connection-string")]
        [Authorize(TenantPermissions.Tenants.ConnectionString)]
        public async Task DeleteDefaultConnectionStringAsync(Guid id)
        {
            await _tenantService.DeleteDefaultConnectionStringAsync(id);
        }
    }
}