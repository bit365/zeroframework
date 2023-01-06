using System.Dynamic;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class Measurement : DynamicObject
    {
        public Dictionary<string, object?> Fields { get; set; } = new();

        public DateTime Timestamp { get; private set; }

        public Measurement(DateTime timestamp) => Timestamp = timestamp;

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
