using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.PermissionProviders;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Products;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    /// <summary>
    /// For more information on enabling Web API for empty projects
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementUnitsController(IMeasurementUnitApplicationService productService) : ControllerBase
    {
        private readonly IMeasurementUnitApplicationService _productService = productService;

        // GET: api/<MeasurementUnitsController>
        [HttpGet]
        [Authorize(ProductPermissions.MeasurementUnits.Default)]
        public async Task<PagedResponseModel<MeasurementUnitGetResponseModel>> GetMeasurementUnits([FromQuery] MeasurementUnitPagedRequestModel model)
        {
            return await _productService.GetListAsync(model);
        }

        // GET api/<MeasurementUnitsController>/5
        [HttpGet("{id}")]
        [Authorize(ProductPermissions.MeasurementUnits.Default)]
        public async Task<MeasurementUnitGetResponseModel> GetMeasurementUnit(int id)
        {
            return await _productService.GetAsync(id);
        }

        // POST api/<MeasurementUnitsController>
        [HttpPost]
        [Authorize(ProductPermissions.MeasurementUnits.Create)]
        public async Task<MeasurementUnitGetResponseModel> PostMeasurementUnit([FromBody] MeasurementUnitCreateRequestModel value)
        {
            return await _productService.CreateAsync(value);
        }

        // PUT api/<MeasurementUnitsController>/5
        [HttpPut("{id}")]
        [Authorize(ProductPermissions.MeasurementUnits.Edit)]
        public async Task<MeasurementUnitGetResponseModel> PutMeasurementUnit(int id, [FromBody] MeasurementUnitUpdateRequestModel value)
        {
            value.Id = id;
            return await _productService.UpdateAsync(id, value);
        }

        // DELETE api/<MeasurementUnitsController>/5
        [HttpDelete("{id}")]
        [Authorize(ProductPermissions.MeasurementUnits.Delete)]
        public async Task DeleteMeasurementUnit(int id)
        {
            await _productService.DeleteAsync(id);
        }
    }
}