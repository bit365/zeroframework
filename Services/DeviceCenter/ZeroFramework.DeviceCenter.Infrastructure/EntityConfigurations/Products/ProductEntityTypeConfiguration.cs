using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityConfigurations.Products
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", Constants.DbConstants.DefaultTableSchema);

            builder.HasKey(e => e.Id);
            builder.Ignore(e => e.DomainEvents);
            builder.Property(e => e.Name).HasMaxLength(20);
            builder.Property(e => e.Remark).HasMaxLength(100);

            JsonSerializerOptions serializeOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            serializeOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            var converter = new ValueConverter<ProductFeatures?, string>(e => JsonSerializer.Serialize(e, serializeOptions), s => JsonSerializer.Deserialize<ProductFeatures?>(s, serializeOptions));

            builder.Property(e => e.Features).HasConversion(converter);

            foreach (var entityType in builder.Metadata.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType.BaseType == typeof(Enum))
                    {
                        var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var enumConverter = Activator.CreateInstance(converterType, new ConverterMappingHints()) as ValueConverter;

                        property.SetMaxLength(20);
                        property.SetValueConverter(enumConverter);
                    }
                }
            }
        }
    }
}