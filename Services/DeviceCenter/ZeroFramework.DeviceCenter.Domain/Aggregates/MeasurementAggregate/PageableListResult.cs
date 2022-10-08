namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class PageableListResult<T>
    {
        public PageableListResult(IReadOnlyList<T>? items, int? nextOffset)
        {
            Items = items;
            NextOffset = nextOffset;
        }

        public IReadOnlyList<T>? Items { get; }

        public int? NextOffset { get; }
    }
}
