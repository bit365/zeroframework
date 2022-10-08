using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Entities
{
    public abstract class BaseEntity
    {
        public abstract object[] GetKeys();
    }

    public abstract class BaseEntity<TKey> : BaseEntity
    {
        [AllowNull]
        public TKey Id { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { Id! };
        }
    }
}