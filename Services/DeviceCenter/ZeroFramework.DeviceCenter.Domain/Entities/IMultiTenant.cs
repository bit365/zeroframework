namespace ZeroFramework.DeviceCenter.Domain.Entities
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; set; }
    }
}
