using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Buyers
{
    public class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.ToTable("Buyers", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(b => b.Id);
            builder.Ignore(b => b.DomainEvents);
            builder.HasIndex(b => b.UserId).IsUnique(true);
            builder.HasMany(b => b.PaymentMethods).WithOne().HasForeignKey("BuyerId").OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}