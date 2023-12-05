namespace ZeroFramework.DeviceCenter.Domain.Exceptions
{
    public class EntityNotFoundException(Type entityType) : Exception
    {
        public Type EntityType { get; set; } = entityType;

        public override string ToString()
        {
            return $"There is no such an entity given given id. Entity type: {EntityType.FullName}";
        }
    }
}
