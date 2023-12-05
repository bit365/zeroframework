using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    public class CustomSaveChangesInterceptor(ICurrentTenant currentTenant) : SaveChangesInterceptor
    {
        private readonly ICurrentTenant _currentTenant = currentTenant;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
            }
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
            }
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void MultiTenancyTracking(DbContext dbContext)
        {
            var tenantedEntries = dbContext.ChangeTracker.Entries<IMultiTenant>().Where(entry => entry.State == EntityState.Added);
            tenantedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Entity.TenantId ??= _currentTenant.Id;
            });
        }
    }
}