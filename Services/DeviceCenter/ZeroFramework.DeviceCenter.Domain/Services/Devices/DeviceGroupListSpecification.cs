using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Constants;
using ZeroFramework.DeviceCenter.Domain.Specifications;
using ZeroFramework.DeviceCenter.Domain.Specifications.Builder;

namespace ZeroFramework.DeviceCenter.Domain.Services.Devices
{
    public class DeviceGroupListSpecification : Specification<DeviceGroup>
    {
        public DeviceGroupListSpecification(int? parentId, string? keyword, int pageNumber = 0, int? pageSize = PagingConstants.DefaultPageSize)
        {
            if (parentId.HasValue)
            {
                Query.Where(e => e.ParentId == parentId);
            }

            if (keyword is not null && !string.IsNullOrWhiteSpace(keyword))
            {
                Query.Where(e => e.Name.Contains(keyword));
            }

            Query.Where(e => e.ParentId == parentId);

            pageSize ??= PagingConstants.DefaultPageSize;

            Query.OrderBy(e => e.Id).Skip((pageNumber - 1) * pageSize.Value).Take(pageSize.Value);

            Query.Include(e => e.Parent).Include(e => e.Children);
            Query.Include(e => e.Children);
        }
    }
}
