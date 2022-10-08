namespace ZeroFramework.DeviceCenter.Domain.Specifications.Builder
{
    public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
    {
    }
}
