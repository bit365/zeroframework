using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    public class CustomDbCommandInterceptor : DbCommandInterceptor
    {
        public override ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }
    }
}
