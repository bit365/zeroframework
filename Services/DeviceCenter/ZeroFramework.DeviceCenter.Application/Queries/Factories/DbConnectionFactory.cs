using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings;

namespace ZeroFramework.DeviceCenter.Application.Queries.Factories
{
    public class DbConnectionFactory(IConnectionStringProvider connectionStringProvider) : IDbConnectionFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider = connectionStringProvider;

        static DbConnectionFactory() => DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

        public async Task<IDbConnection> CreateConnection(string? nameOrConnectionString)
        {
            string connectionString = await _connectionStringProvider.GetAsync(nameOrConnectionString);

            DbConnection? dbConnection = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection() ?? throw new ArgumentException("Unable to find the requested database provider. It may not be installed.");

            dbConnection.ConnectionString = connectionString;

            return dbConnection;
        }
    }
}
