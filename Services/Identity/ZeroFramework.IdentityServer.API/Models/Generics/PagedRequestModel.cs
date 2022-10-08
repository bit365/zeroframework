using Microsoft.AspNetCore.Mvc;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Extensions;

namespace ZeroFramework.IdentityServer.API.Models.Generics
{
    public record PagedRequestModel
    {
        [ModelBinder(BinderType = typeof(SortingModelBinder))]
        public virtual IEnumerable<SortingDescriptor>? Sorter { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = PagingConstants.DefaultPageSize;
    }
}