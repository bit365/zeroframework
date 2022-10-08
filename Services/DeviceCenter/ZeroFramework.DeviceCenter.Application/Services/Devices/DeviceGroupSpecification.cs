using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Specifications;
using ZeroFramework.DeviceCenter.Domain.Specifications.Builder;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public class DeviceGroupSpecification : Specification<DeviceGroup>
    {
        public DeviceGroupSpecification(int pageNumber, int pageSize)
        {
            Query.Include(e => e.Children);
            Query.Include(e => e.Parent);
            Query.OrderByDescending(o => o.CreationTime);
            Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}