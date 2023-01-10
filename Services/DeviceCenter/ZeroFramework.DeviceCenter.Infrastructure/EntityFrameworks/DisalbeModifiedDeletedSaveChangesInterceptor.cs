using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class DisalbeModifiedDeletedSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var entries = eventData.Context?.ChangeTracker.Entries();

            if (entries is not null)
            {
                foreach (var entity in entries)
                {
                    if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted)
                    {
                        throw new InvalidOperationException("DisalbeModifiedDeleted");
                    }
                }
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var entries = eventData.Context?.ChangeTracker.Entries();

            if (entries is not null)
            {
                foreach (var entity in entries)
                {
                    if (entity.State == EntityState.Modified || entity.State == EntityState.Deleted)
                    {
                        throw new InvalidOperationException("DisalbeModifiedDeleted");
                    }
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
