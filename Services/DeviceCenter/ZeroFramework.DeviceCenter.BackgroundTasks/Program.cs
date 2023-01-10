using NLog.Extensions.Logging;
using ZeroFramework.DeviceCenter.Application;
using ZeroFramework.DeviceCenter.BackgroundTasks.Services;
using ZeroFramework.DeviceCenter.Domain;
using ZeroFramework.DeviceCenter.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {

        services.AddHttpClient();
        services.AddDomainLayer();
        services.AddInfrastructureLayer(context.Configuration);
        services.AddApplicationLayer(context.Configuration);
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddLogging(options => options.ClearProviders().AddNLog());

        services.AddHttpClient();
        services.AddHostedService<MockSampleWorker>();
    })
    .Build();

host.Run();
