using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class DeviceCenterEfCoreRepository<TEntity>(DeviceCenterDbContext dbContext) : EfCoreRepository<DeviceCenterDbContext, TEntity>(dbContext) where TEntity : BaseEntity
    {
    }

    public class DeviceCenterEfCoreRepository<TEntity, TKey>(DeviceCenterDbContext dbContext) : EfCoreRepository<DeviceCenterDbContext, TEntity, TKey>(dbContext) where TEntity : BaseEntity<TKey>
    {
    }
}
