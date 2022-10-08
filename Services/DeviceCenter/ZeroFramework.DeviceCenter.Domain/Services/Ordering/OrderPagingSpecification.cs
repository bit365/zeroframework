using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Specifications;
using ZeroFramework.DeviceCenter.Domain.Specifications.Builder;

namespace ZeroFramework.DeviceCenter.Domain.Services.Ordering
{
    public class OrderPagingSpecification : Specification<Order>
    {
        public OrderPagingSpecification(int pageNumber, int pageSize)
        {
            Query.Where(o => o.CreationTime > DateTimeOffset.Now.AddDays(-1));
            Query.Include(o => o.OrderItems);
            Query.Include(o => o.Address).ThenInclude(d => d.Country);
            Query.OrderBy(o => o.CreationTime);
            Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}