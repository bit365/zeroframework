namespace ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings
{
    public interface IConnectionStringProvider
    {
        Task<string> GetAsync(string? connectionStringName = null);
    }
}