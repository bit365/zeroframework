using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.ResourceGroups
{
    public class ResourceGroupEntityTypeConfiguration : IEntityTypeConfiguration<ResourceGroup>
    {
        public void Configure(EntityTypeBuilder<ResourceGroup> builder)
        {
            builder.ToTable("ResourceGroups", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(20);
            builder.Property(t => t.DisplayName).HasMaxLength(256);

            builder.HasIndex(e => new { e.Name, e.TenantId }).IsUnique();

            builder.HasMany<PermissionGrant>().WithOne().HasForeignKey(e => e.ResourceGroupId);
        }
    }
}