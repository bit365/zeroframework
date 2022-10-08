using Microsoft.EntityFrameworkCore;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;

namespace ZeroFramework.DeviceCenter.Infrastructure.Repositories
{
    public class PermissionGrantRepository : EfCoreRepository<DeviceCenterDbContext, PermissionGrant, Guid>, IPermissionGrantRepository
    {
        public PermissionGrantRepository(DeviceCenterDbContext dbContext) : base(dbContext) { }

        public async Task<PermissionGrant?> FindAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default)
        {
            if (resourceGroupId == Guid.Empty)
            {
                return await DbSet.OrderBy(x => x.Id).FirstOrDefaultAsync(e => e.OperationName == operationName && e.ProviderName == providerName && e.ProviderKey == providerKey, cancellationToken);
            }

            return await DbSet.OrderBy(x => x.Id).FirstOrDefaultAsync(e => e.OperationName == operationName && e.ProviderName == providerName && e.ProviderKey == providerKey && e.ResourceGroupId == resourceGroupId, cancellationToken);
        }

        public async Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default)
        {
            if (resourceGroupId == Guid.Empty)
            {
                return await DbSet.Where(e => e.ProviderName == providerName && e.ProviderKey == providerKey).ToListAsync(cancellationToken);
            }

            return await DbSet.Where(e => e.ProviderName == providerName && e.ProviderKey == providerKey && e.ResourceGroupId == resourceGroupId).ToListAsync(cancellationToken);
        }

        public async Task<List<PermissionGrant>> GetListAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default)
        {
            if (resourceGroupId == Guid.Empty)
            {
                return await DbSet.Where(e => operationNames.Contains(e.OperationName) && e.ProviderName == providerName && e.ProviderKey == providerKey).ToListAsync(cancellationToken);
            }

            return await DbSet.Where(e => operationNames.Contains(e.OperationName) && e.ProviderName == providerName && e.ProviderKey == providerKey && e.ResourceGroupId == resourceGroupId).ToListAsync(cancellationToken);
        }
    }
}