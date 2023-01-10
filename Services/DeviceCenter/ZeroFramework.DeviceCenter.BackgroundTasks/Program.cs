using NLog.Extensions.Logging;
using ZeroFramework.DeviceCenter.Domain;
using ZeroFramework.DeviceCenter.Infrastructure;
using ZeroFramework.DeviceCenter.Application;
using ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.AddHttpClient();
        services.AddDomainLayer();
        services.AddInfrastructureLayer(context.Configuration);
        services.AddApplicationLayer(context.Configuration);
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddLogging(options => options.ClearProviders().AddNLog());

        services.AddHostedService<YuanGanWorker>();
    })
    .Build();

host.Run();
