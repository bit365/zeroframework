namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class PageableListResposeModel<T>(IReadOnlyList<T> items, int? offset)
    {
        public IReadOnlyList<T> Items { get; } = items;

        public int? Offset { get; } = offset;
    }
}