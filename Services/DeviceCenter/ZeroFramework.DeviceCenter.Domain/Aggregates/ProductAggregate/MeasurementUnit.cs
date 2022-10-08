using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class MeasurementUnit : BaseAggregateRoot<int>
    {
        [AllowNull]
        public string UnitName { get; set; }

        [AllowNull]
        public string Unit { get; set; }

        [AllowNull]
        public string? Remark { get; set; }
    }
}
