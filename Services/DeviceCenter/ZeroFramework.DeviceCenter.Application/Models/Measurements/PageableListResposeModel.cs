namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class PageableListResposeModel<T>
    {
        public PageableListResposeModel(IReadOnlyList<T> items, int? offset)
        {
            Items = items;
            Offset = offset;
        }

        public IReadOnlyList<T> Items { get; }

        public int? Offset { get; }
    }
}