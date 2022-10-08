using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ZeroFramework.DeviceCenter.Infrastructure.Constants;

namespace ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks
{
    public class DeviceCenterDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DeviceCenterDbContext>
    {
        public DeviceCenterDbContext CreateDbContext(string[] args)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../ZeroFramework.DeviceCenter.API");

            ConfigurationBuilder configurationBuilder = new();

            configurationBuilder.SetBasePath(basePath);

            configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (environment is not null)
            {
                configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true);
            }

            IConfiguration configuration = configurationBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<DeviceCenterDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DefaultConnectionStringName));

            return new DeviceCenterDbContext(optionsBuilder.Options);
        }
    }
}