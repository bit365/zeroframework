using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public interface IMeasurementRepository
    {
        Task AddMeasurementsAsync(int productId, long deviceId, FeatureType featureType, string identifier, params Measurement[] measurements);

        Task<PageableListResult<Measurement>?> GetMeasurementsAsync(int productId, long deviceId, FeatureType? featureType, string? identifier, DateTime startTime, DateTime endTime, bool hoursFirst = false, bool descending = false, int offset = 0, int count = 10);

        Task<PageableListResult<TelemetryAggregate>?> GetTelemetryAggregatesAsync(int productId, long deviceId, string identifier, DateTime startTime, DateTime endTime, string timeInterval, int offset, int count);

        Task<IEnumerable<TelemetryValue>?> GetTelemetryValuesAsync(int productId, long deviceId);

        Task SetTelemetryValueAsync(int productId, long deviceId, params TelemetryValue[] telemetryValues);
    }
}