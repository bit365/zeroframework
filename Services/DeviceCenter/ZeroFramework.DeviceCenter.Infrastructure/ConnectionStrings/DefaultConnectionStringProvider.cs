using Microsoft.Extensions.Configuration;
using ZeroFramework.DeviceCenter.Infrastructure.Constants;

namespace ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings
{
    public class DefaultConnectionStringProvider(IConfiguration configuration) : IConnectionStringProvider
    {
        protected readonly IConfiguration _configuration = configuration;

        public virtual Task<string> GetAsync(string? connectionStringName = null)
        {
            connectionStringName ??= DbConstants.DefaultConnectionStringName;

            return Task.FromResult(_configuration.GetConnectionString(connectionStringName)!);
        }
    }
}
