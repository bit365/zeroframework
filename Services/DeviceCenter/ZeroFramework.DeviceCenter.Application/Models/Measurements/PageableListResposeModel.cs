namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class PageableListResposeModel<T>
    {
        public PageableListResposeModel(IReadOnlyList<T>? items, int? nextOffset)
        {
            Items = items;
            NextOffset = nextOffset;
        }

        public IReadOnlyList<T>? Items { get; }

        public int? NextOffset { get; }
    }
}