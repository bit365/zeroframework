namespace ZeroFramework.DeviceCenter.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get; set; }

        public EntityNotFoundException(Type entityType)
        {
            EntityType = entityType;
        }

        public override string ToString()
        {
            return $"There is no such an entity given given id. Entity type: {EntityType.FullName}";
        }
    }
}
