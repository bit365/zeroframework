using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class PropertyFeature : AbstractFeature
    {
        [AllowNull]
        public DataType DataType { get; set; }

        public PropertyAccessMode AccessMode { get; set; }
    }
}
