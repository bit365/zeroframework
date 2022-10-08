namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public class PagedResponseModel<T>
    {
        public PagedResponseModel(IReadOnlyList<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        public IReadOnlyList<T> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
