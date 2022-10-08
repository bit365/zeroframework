using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Ordering
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.DomainEvents);

            var converter = new ValueConverter<OrderStatus, string>(v => v.ToString(), v => OrderStatus.FromName(v));
            builder.Property(o => o.OrderStatus).HasConversion(converter);
            builder.Metadata.FindNavigation(nameof(Order.OrderItems))?.SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.OwnsOne(o => o.Address, a => a.WithOwner());
            builder.HasOne<PaymentMethod>().WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Buyer>().WithMany().IsRequired(false).HasForeignKey(o => o.BuyerId);
        }
    }
}
