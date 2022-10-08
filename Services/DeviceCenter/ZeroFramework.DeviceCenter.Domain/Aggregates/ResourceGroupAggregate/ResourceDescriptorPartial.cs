using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate
{
    public partial record ResourceDescriptor
    {
        public static implicit operator string(ResourceDescriptor resourceDescriptor) => resourceDescriptor.ToString();

        public ResourceDescriptor(Type resourceType, object resourceId)
        {
            ResourceType = resourceType.Name;
            ResourceId = resourceId.ToString();
        }

        public ResourceDescriptor(object resource, object resourceId)
        {
            ResourceType = resource.GetType().Name;
            ResourceId = resourceId.ToString();
        }

        public ResourceDescriptor(BaseEntity resource)
        {
            ResourceType = resource.GetType().Name;
            ResourceId = resource.GetKeys().Single().ToString();
        }

        public ResourceDescriptor(string resourceType, string resourceId)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }

        public static explicit operator ResourceDescriptor(string str)
        {
            string[] stringSeparators = str.Split(":");
            ResourceDescriptor resourceObject = new(stringSeparators.First(), stringSeparators.Last());
            return resourceObject;
        }

        public override string ToString() => $"{ResourceType}:{ResourceId}";
    }
}
