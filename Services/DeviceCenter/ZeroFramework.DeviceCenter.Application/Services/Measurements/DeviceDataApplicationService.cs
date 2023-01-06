using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Constants;

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

        public async Task SetDevicePropertyValues(int productId, long deviceId, IDictionary<string, DevicePropertyValue> values)
        {
            var telemetryValues = values.Select(e => new TelemetryValue { Identifier = e.Key, Timestamp = e.Value.Timestamp ?? DateTimeOffset.Now.ToUnixTimeMilliseconds(), Value = e.Value });

            await _repository.SetTelemetryValueAsync(productId, deviceId, telemetryValues.ToArray());

            foreach (var item in telemetryValues)
            {
                dynamic measurement = new Measurement(DateTimeOffset.FromUnixTimeMilliseconds(item.Timestamp).LocalDateTime);

                measurement.Value = item.Value;

                await _repository.AddMeasurementsAsync(productId, deviceId, FeatureType.Property, item.Identifier, measurement);
            }
        }

        public async Task<IEnumerable<DevicePropertyLastValue>?> GetDevicePropertyValues(int productId, long deviceId)
        {
            var telemetryValues = await _repository.GetTelemetryValuesAsync(productId, deviceId);

            if (telemetryValues is not null)
            {
                ProductGetResponseModel productGetResponseModel = await _productApplicationService.GetAsync(productId);

                telemetryValues ??= Enumerable.Empty<TelemetryValue>().ToList();

                List<DevicePropertyLastValue> devicePropertyLastValues = new();

                foreach (var item in telemetryValues)
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

        public async Task<PageableListResposeModel<DevicePropertyValue>?> GetDevicePropertyHistoryValues(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, bool hoursFirst = false, SortingOrder sorting = SortingOrder.Ascending, int offset = 0, int count = PagingConstants.DefaultPageSize)
        {
            bool isDescending = sorting == SortingOrder.Descending;

            var measurements = await _repository.GetMeasurementsAsync(productId, deviceId, FeatureType.Property, identifier, startTime.LocalDateTime, endTime.LocalDateTime, hoursFirst, isDescending, offset, count);

            var list = measurements?.Items?.Select(e => new DevicePropertyValue { Timestamp = new DateTimeOffset(e.Timestamp).ToUnixTimeMilliseconds(), Value = e.Fields["Value"] }).ToList();

            return list is null ? null : new PageableListResposeModel<DevicePropertyValue>(list, measurements?.Offset);
        }

        public async Task<PageableListResposeModel<DevicePropertyReport>?> GetDevicePropertyReports(int productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int offset = 0, int count = PagingConstants.DefaultPageSize)
        {
            var aggregates = await _repository.GetTelemetryAggregatesAsync(productId, deviceId, identifier, startTime.LocalDateTime, endTime.LocalDateTime, reportType, offset, count);

            var list = aggregates?.Items?.Select(e => new DevicePropertyReport { Time = e.Time, Min = e.Min, Average = e.Average, Max = e.Max, Count = e.Count }).ToList();

            return list is null ? null : new PageableListResposeModel<DevicePropertyReport>(list, aggregates?.Offset);
        }
    }
}