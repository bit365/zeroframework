using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Permissions
{
    public class PermissionGrantEntityTypeConfiguration : IEntityTypeConfiguration<PermissionGrant>
    {
        public void Configure(EntityTypeBuilder<PermissionGrant> builder)
        {
            builder.ToTable("PermissionGrants", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(e => e.Id);

            foreach (IMutableEntityType entityType in builder.Metadata.Model.GetEntityTypes())
            {
                if (entityType.ClrType == typeof(PermissionGrant))
                {
                    foreach (IMutableProperty property in entityType.GetProperties().Where(p => p.ClrType == typeof(string)))
                    {
                        property.SetMaxLength(36);
                        builder.Property(property.Name).IsRequired(true);
                    }
                }
            }

            builder.Property(e => e.OperationName).IsRequired().HasMaxLength(byte.MaxValue);

            builder.HasIndex(e => new { e.OperationName, e.ProviderName, e.ProviderKey, e.ResourceGroupId, e.TenantId }).IsUnique();
        }
    }
}