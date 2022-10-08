using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate
{
    public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
    {
        Task<PermissionGrant?> FindAsync(string operationName, string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default);

        Task<List<PermissionGrant>> GetListAsync(string[] operationNames, string providerName, string providerKey, Guid? resourceGroupId, CancellationToken cancellationToken = default);
    }
}