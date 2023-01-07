using NLog.Extensions.Logging;
using ZeroFramework.DeviceCenter.BackgroundTasks;
using ZeroFramework.DeviceCenter.Domain;
using ZeroFramework.DeviceCenter.Infrastructure;
using ZeroFramework.DeviceCenter.Application;
using ZeroFramework.DeviceCenter.BackgroundTasks.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
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
