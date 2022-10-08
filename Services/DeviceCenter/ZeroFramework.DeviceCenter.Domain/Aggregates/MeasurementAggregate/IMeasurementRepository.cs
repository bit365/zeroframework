using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public interface IMeasurementRepository
    {
        Task AddMeasurementAsync(Guid productId, long deviceId, FeatureType featureType, string identifier, Measurement measurement);

        Task<IEnumerable<Measurement>> GetLastMeasurementsAsync(Guid productId, long deviceId, FeatureType featureType);

        Task<PageableListResult<Measurement>> GetMeasurementsAsync(Guid productId, long deviceId, FeatureType? featureType, string? identifier, DateTimeOffset startTime, DateTimeOffset endTime, bool ascending, int skip, int top);

        Task<PageableListResult<TelemetryAggregate>> GetTelemetryAggregatesAsync(Guid productId, long deviceId, string identifier, DateTimeOffset startTime, DateTimeOffset endTime, string reportType, int skip, int top);

        Task<TelemetryValueList> GetTelemetryValuesAsync(Guid productId, long deviceId);

        Task SetTelemetryValueAsync(Guid productId, long deviceId, string identifier, DateTimeOffset timestamp, object value);
    }
}