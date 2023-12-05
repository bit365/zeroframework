using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings
{
    public class TenantDbConnectionInterceptor(IConnectionStringProvider connectionStringProvider) : DbConnectionInterceptor
    {
        private readonly IConnectionStringProvider _connectionStringProvider = connectionStringProvider;

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            connection.ConnectionString = _connectionStringProvider.GetAsync().Result;
            return base.ConnectionOpening(connection, eventData, result);
        }

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default)
        {
            connection.ConnectionString = await _connectionStringProvider.GetAsync();
            return await base.ConnectionOpeningAsync(connection, eventData, result, cancellationToken);
        }
    }
}
