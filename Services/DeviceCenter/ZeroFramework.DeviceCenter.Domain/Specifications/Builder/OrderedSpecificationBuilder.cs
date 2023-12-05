namespace ZeroFramework.DeviceCenter.Domain.Specifications.Builder
{
    public class OrderedSpecificationBuilder<T>(Specification<T> specification) : IOrderedSpecificationBuilder<T>
    {
        public Specification<T> Specification { get; } = specification;
    }
}
