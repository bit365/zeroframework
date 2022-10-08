using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public abstract class AbstractFeature
    {
        [AllowNull]
        public string Identifier { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? Desc { get; set; }
    }
}