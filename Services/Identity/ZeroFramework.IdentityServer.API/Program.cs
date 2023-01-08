using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseNLog();

    // Add services to the container.

    var startup = new ZeroFramework.IdentityServer.API.Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    startup.Configure(app, app.Environment);

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}