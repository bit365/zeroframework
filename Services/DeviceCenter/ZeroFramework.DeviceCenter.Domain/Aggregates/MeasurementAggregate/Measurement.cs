using System.Dynamic;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class Measurement : DynamicObject
    {
        public DateTime Timestamp { get; private set; }

        public IDictionary<string, object?> Fields { get; set; } = new Dictionary<string, object?>();

        public Measurement(DateTimeOffset timestamp) => Timestamp = timestamp.LocalDateTime;

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            Fields[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            return Fields.TryGetValue(binder.Name, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var item in Fields.Keys)
            {
                yield return item;
            }
        }
    }
}
