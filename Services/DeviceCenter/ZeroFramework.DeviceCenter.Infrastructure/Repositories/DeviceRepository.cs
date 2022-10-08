using Microsoft.EntityFrameworkCore;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Constants;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class DeviceRepository : EfCoreRepository<DeviceCenterDbContext, Device, long>, IDeviceRepository
    {
        public DeviceRepository(DeviceCenterDbContext dbContext) : base(dbContext) { }

        public async Task<int> GetCountAsync(Guid? productId, int? deviceGroupId, DeviceStatus? status, string? deviceName, CancellationToken cancellationToken = default)
        {
            IQueryable<Device> query = DbSet;

            if (deviceGroupId.HasValue)
            {
                query = DbSet.FromSqlInterpolated($"SELECT * FROM Devices WHERE EXISTS (SELECT DeviceId FROM DeviceGroupings WHERE DeviceGroupId={deviceGroupId})");
            }

            if (deviceName is not null && !string.IsNullOrWhiteSpace(deviceName))
            {
                query = query.Where(e => e.Name.Contains(deviceName));
            }

            if (productId.HasValue)
            {
                query = query.Where(e => e.ProductId == productId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            return await query.CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<Device>> GetListAsync(Guid? productId, int? deviceGroupId, DeviceStatus? status, string? deviceName, int pageNumber = 1, int pageSize = PagingConstants.DefaultPageSize, CancellationToken cancellationToken = default)
        {
            IQueryable<Device> query = DbSet;

            if (deviceGroupId.HasValue)
            {
                query = DbSet.FromSqlInterpolated($"SELECT * FROM Devices WHERE EXISTS (SELECT DeviceId FROM DeviceGroupings WHERE DeviceGroupId={deviceGroupId})");
            }

            if (deviceName is not null && !string.IsNullOrWhiteSpace(deviceName))
            {
                query = query.Where(e => e.Name.Contains(deviceName));
            }

            if (productId.HasValue)
            {
                query = query.Where(e => e.ProductId == productId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            query = query.OrderByDescending(e => e.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}