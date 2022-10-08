using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Models.Generics;

namespace ZeroFramework.IdentityServer.API.Extensions
{
    public static class QueryableCustomExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, IEnumerable<SortingDescriptor>? sorter)
        {
            var properties = source.GetType().GetGenericArguments().First().GetProperties();

            IOrderedQueryable<T>? orderedQueryable = null;

            if (sorter is null || !sorter.Any())
            {
                string firstPropertyName = typeof(T).GetProperties().First().Name;
                return source.OrderByDescending(firstPropertyName);
            }

            foreach (SortingDescriptor storingDescriptor in sorter)
            {
                string? propertyName = properties.SingleOrDefault(p => string.Equals(p.Name, storingDescriptor.PropertyName, StringComparison.OrdinalIgnoreCase))?.Name;

                if (propertyName is null)
                {
                    throw new KeyNotFoundException(storingDescriptor.PropertyName);
                }

                if (storingDescriptor.SortDirection == SortingOrder.Ascending)
                {
                    orderedQueryable = orderedQueryable is null ? source.OrderBy(propertyName) : orderedQueryable.ThenBy(propertyName);
                }
                else if (storingDescriptor.SortDirection == SortingOrder.Descending)
                {
                    orderedQueryable = orderedQueryable is null ? source.OrderByDescending(propertyName) : orderedQueryable.ThenByDescending(propertyName);
                }
            }

            return orderedQueryable ?? source;
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, int pageNumber, int pageSize = PagingConstants.DefaultPageSize) => source.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

        public static IQueryable<T> ApplySortingAndPaging<T>(this IQueryable<T> source, PagedRequestModel model)
        {
            return source.ApplySorting(model.Sorter).ApplyPaging(model.PageNumber, model.PageSize);
        }
    }
}
