using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class CustomSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IMediator _mediator;

        public CustomSaveChangesInterceptor(IMediator mediator) => _mediator = mediator;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
                SoftDeleteTracking(eventData.Context);
                DispatchDomainEventsAsync(eventData.Context).Wait();
            }

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
                SoftDeleteTracking(eventData.Context);
                await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            }
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void SoftDeleteTracking(DbContext dbContext)
        {
            var deletedEntries = dbContext.ChangeTracker.Entries().Where(entry => entry.State == EntityState.Deleted && entry.Entity is ISoftDelete);
            deletedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Reload();
                entityEntry.State = EntityState.Modified;
                ((ISoftDelete)entityEntry.Entity).IsDeleted = true;
            });
        }

        private static void MultiTenancyTracking(DbContext dbContext)
        {
            var tenantedEntries = dbContext.ChangeTracker.Entries<IMultiTenant>().Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);
            var currentTenant = dbContext.GetService<ICurrentTenant>();
            tenantedEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Entity.TenantId ??= currentTenant.Id;
            });
        }

        private async Task DispatchDomainEventsAsync(DbContext dbContext, CancellationToken cancellationToken = default)
        {
            var domainEntities = dbContext.ChangeTracker.Entries<IDomainEvents>().Select(e => e.Entity);
            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();
            domainEntities.ToList().ForEach(entity => entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}