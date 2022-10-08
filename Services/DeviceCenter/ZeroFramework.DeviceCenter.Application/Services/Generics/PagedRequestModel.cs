namespace ZeroFramework.DeviceCenter.Application.Services.Generics
{
    public class PagedRequestModel
    {
        public virtual IEnumerable<SortingDescriptor>? Sorter { get; set; }

        public virtual int PageNumber { get; set; } = 1;

        public virtual int PageSize { get; set; } = Domain.Constants.PagingConstants.DefaultPageSize;
    }
}
