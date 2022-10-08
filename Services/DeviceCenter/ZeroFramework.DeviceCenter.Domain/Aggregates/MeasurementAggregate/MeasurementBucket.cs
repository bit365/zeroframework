using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class MeasurementBucket : DynamicObject
    {
        [AllowNull]
        public string Id { get; set; }

        public Guid ProductId { get; set; }

        public long DeviceId { get; set; }

        [AllowNull]
        public FeatureType FeatureType { get; set; }

        [AllowNull]
        public string Identifier { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public IDictionary<string, object?> Metadata { get; set; } = new Dictionary<string, object?>();

        [AllowNull]
        public List<Measurement> Measurements { get; set; }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            Metadata[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return Metadata.TryGetValue(binder.Name, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var item in Metadata.Keys)
            {
                yield return item;
            }
        }
    }
}