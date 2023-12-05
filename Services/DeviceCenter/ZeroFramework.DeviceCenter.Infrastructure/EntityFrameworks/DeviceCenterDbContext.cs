using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;
using ZeroFramework.DeviceCenter.Domain.Entities;
using ZeroFramework.DeviceCenter.Domain.UnitOfWork;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class DeviceCenterDbContext(DbContextOptions<DeviceCenterDbContext> options) : DbContext(options), IUnitOfWork
    {
        async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken) => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    modelBuilder.Entity(entityType.ClrType).AddQueryFilter<IMultiTenant>(e => e.TenantId == this.GetService<ICurrentTenant>().Id);
                }

                if (entityType.ClrType.IsAssignableTo(typeof(ISoftDelete)))
                {
                    modelBuilder.Entity(entityType.ClrType).AddQueryFilter<ISoftDelete>(e => !e.IsDeleted);
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}