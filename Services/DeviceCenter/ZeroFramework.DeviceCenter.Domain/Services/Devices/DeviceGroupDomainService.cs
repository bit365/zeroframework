using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Domain.Services.Devices
{
    public class DeviceGroupDomainService(IRepository<DeviceGroup, int> deviceGroupRepository) : IDeviceGroupDomainService
    {
        private readonly IRepository<DeviceGroup, int> _deviceGroupRepository = deviceGroupRepository;

        public async Task<(List<DeviceGroup> Items, int TotalCount)> GetDeviceGroupListAsync(int? parentId, string? keyword, int pageNumber, int pageSize)
        {
            var specification = new DeviceGroupListSpecification(parentId, keyword, pageNumber, pageSize);

            var items = await _deviceGroupRepository.GetListAsync(specification);

            IQueryable<DeviceGroup> queryable = _deviceGroupRepository.Query;
            specification.WhereExpressions.ToList().ForEach(e => queryable = queryable.Where(e));

            int totalCount = await _deviceGroupRepository.AsyncExecuter.CountAsync(queryable);

            return (items, totalCount);
        }
    }
}
