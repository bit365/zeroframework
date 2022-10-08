using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestsController : ControllerBase
    {
        private readonly IValidator<ProjectCreateOrUpdateRequestModel> _validator;

        private readonly IDeviceDataApplicationService _deviceDataApplicationService;

        public TestsController(IValidator<ProjectCreateOrUpdateRequestModel> validator, IDeviceDataApplicationService deviceDataApplicationService)
        {
            _validator = validator;
            _deviceDataApplicationService = deviceDataApplicationService;
        }

        [HttpGet]
        public async Task<ActionResult<Measurement>> Get()
        {
            Guid productId = Guid.Parse("b4b9996c-beb5-4695-ad91-072eac1a6f89");

            long deviceId = 10000;

            Random random = new();

            DateTimeOffset currentDateTime = DateTimeOffset.Now;

            for (int i = 1; i < 45; i++)
            {
                currentDateTime = currentDateTime.AddMinutes(random.Next(5, 20));

                Measurement value = new(currentDateTime);

                value.Fields.Add("Value", i);

                await _deviceDataApplicationService.SetDevicePropertyValue(productId, deviceId, "Abc", new DevicePropertyValue
                {
                    Timestamp = currentDateTime.ToUnixTimeMilliseconds(),
                    Value = i
                });
            }

            return Ok("OK");
        }

        [HttpPost]
        public async Task<ActionResult<ProjectGetResponseModel>> Post([FromBody] ProjectCreateOrUpdateRequestModel value)
        {
            ValidationResult validationResult = _validator.Validate(value);

            if (validationResult.IsValid)
            {
                return await Task.FromResult(new ProjectGetResponseModel());
            }

            return BadRequest(validationResult);
        }
    }
}
