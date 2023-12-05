namespace ZeroFramework.DeviceCenter.Domain.Specifications.Builder
{
    public class IncludableSpecificationBuilder<T, TProperty>(Specification<T> specification) : IIncludableSpecificationBuilder<T, TProperty> where T : class
    {
        public Specification<T> Specification { get; } = specification;
    }
}
