namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class PageableListResult<T>(IReadOnlyList<T> items, int? offset)
    {
        public IReadOnlyList<T> Items { get; } = items;

        public int? Offset { get; } = offset;
    }
}
