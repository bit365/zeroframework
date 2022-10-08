using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Application.Services.Measurements
{
    public class DeviceDataApplicationService : IDeviceDataApplicationService
    {
        private readonly IMeasurementRepository _repository;

        private readonly IMapper _mapper;

        private readonly IProductApplicationService _productApplicationService;

        public DeviceDataApplicationService(IMeasurementRepository repository, IMapper mapper, IProductApplicationService productApplicationService)
        {
            _repository = repository;
            _mapper = mapper;
            _productApplicationService = productApplicationService;
        }

        public async Task SetDevicePropertyValue(Guid productId, long deviceId, string identifier, DevicePropertyValue propertyValue)
        {
            dynamic measurement = new Measurement(DateTimeOffset.FromUnixTimeMilliseconds(propertyValue.Timestamp));

            measurement.Value = propertyValue.Value;

            await _repository.SetTelemetryValueAsync(productId, deviceId, identifier, measurement.Timestamp, propertyValue.Value);
            await _repository.AddMeasurementAsync(productId, deviceId, FeatureType.Property, identifier, measurement);
        }

        public async Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(Guid productId, long deviceId)
        {
            var telemetryValues = await _repository.GetTelemetryValuesAsync(productId, deviceId);

            if (telemetryValues is not null)
            {
                ProductGetResponseModel productGetResponseModel = await _productApplicationService.GetAsync(productId);

                telemetryValues.Values ??= Enumerable.Empty<TelemetryValue>().ToList();

                List<DevicePropertyLastValue> devicePropertyLastValues = new();

                foreach (var item in telemetryValues.Values)
                {
                    var property = productGetResponseModel?.Features?.Properties?.SingleOrDefault(p => p.Identifier == item.Identifier);

                    if (property is not null)
                    {
                        dynamic? specs = property.DataType?.Specs;

                        devicePropertyLastValues.Add(new DevicePropertyLastValue
                        {
                            Identifier = item.Identifier,
                            Name = property?.Name,
                            Unit = specs?.unit?.ToString(),
                            Timestamp = item.Timestamp,
                            Value = item.Value
                        });
                    }
                }

                return devicePropertyLastValues;
            }

            return null;
        }

        public async Task<PageableListResposeModel<DevicePropertyValue>> GetDevicePropertyHistoryValues(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, SortingOrder sorting, int skip, int top)
        {
            var result = await _repository.GetMeasurementsAsync(productId, deviceId, FeatureType.Property, identifier, startTime, endTime, sorting == SortingOrder.Ascending, skip, top);

            var list = result?.Items?.Select(e => new DevicePropertyValue { Timestamp = new DateTimeOffset(e.Timestamp).ToUnixTimeMilliseconds(), Value = e.Fields["Value"] }).ToList();

            return new PageableListResposeModel<DevicePropertyValue>(list, result?.NextOffset);
        }

        public async Task<PageableListResposeModel<DevicePropertyReport>> GetDevicePropertyReports(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int skip, int top)
        {
            var result = await _repository.GetTelemetryAggregatesAsync(productId, deviceId, identifier, startTime, endTime, reportType, skip, top);

            var list = result?.Items?.Select(e => _mapper.Map<DevicePropertyReport>(e)).ToList();

            return new PageableListResposeModel<DevicePropertyReport>(list, result?.NextOffset);
        }
    }
}
