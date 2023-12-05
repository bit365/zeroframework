namespace ZeroFramework.IdentityServer.API.Models.Generics
{
    public class PagedResponseModel<T>(IReadOnlyList<T> items, int totalCount)
    {
        public IReadOnlyList<T> Items { get; set; } = items;

        public int TotalCount { get; set; } = totalCount;
    }
}
