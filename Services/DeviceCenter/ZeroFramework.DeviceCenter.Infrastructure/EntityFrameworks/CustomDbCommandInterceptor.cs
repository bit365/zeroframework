using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class CustomDbCommandInterceptor : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}