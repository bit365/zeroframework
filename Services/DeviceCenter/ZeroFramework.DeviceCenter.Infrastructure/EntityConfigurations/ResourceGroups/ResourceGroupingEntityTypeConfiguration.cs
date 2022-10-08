using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.ResourceGroups
{
    public class ResourceGroupingEntityTypeConfiguration : IEntityTypeConfiguration<ResourceGrouping>
    {
        public void Configure(EntityTypeBuilder<ResourceGrouping> builder)
        {
            builder.ToTable("ResourceGroupings", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(e => e.Id);
            builder.OwnsOne(e => e.Resource, ownBuilder =>
            {
                ownBuilder.Property(e => e.ResourceType).HasColumnName(nameof(ResourceDescriptor.ResourceType)).HasMaxLength(byte.MaxValue);
                ownBuilder.Property(e => e.ResourceId).HasColumnName(nameof(ResourceDescriptor.ResourceId)).HasMaxLength(byte.MaxValue);
            });

            builder.HasOne<ResourceGroup>().WithMany().HasForeignKey(e => e.ResourceGroupId);
        }
    }
}
