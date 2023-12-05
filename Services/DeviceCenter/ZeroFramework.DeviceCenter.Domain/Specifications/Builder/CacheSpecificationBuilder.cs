namespace ZeroFramework.DeviceCenter.Domain.Specifications.Builder
{
    public class CacheSpecificationBuilder<T>(Specification<T> specification) : ICacheSpecificationBuilder<T> where T : class
    {
        public Specification<T> Specification { get; } = specification;
    }
}
