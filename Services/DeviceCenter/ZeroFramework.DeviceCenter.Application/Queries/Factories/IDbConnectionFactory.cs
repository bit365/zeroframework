using System.Data;

namespace ZeroFramework.DeviceCenter.Application.Queries.Factories
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnection(string? nameOrConnectionString = null);
    }
}
