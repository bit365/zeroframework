using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Channels;
using System.Text;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class HuanJingWorker : BackgroundService
    {
        private readonly IRepository<Product, int> _productRepository;

        private readonly IRepository<Device, long> _deviceRepository;

        private readonly IDeviceDataApplicationService _deviceApplicationService;

        private readonly ILogger<YuanGanWorker> _logger;

        private readonly HttpClient _httpClient;

        private readonly IRepository<DeviceGroup, int> _deviceGroupRepository;

        private readonly IRepository<DeviceGrouping> _deviceGroupingRepository;

        public HuanJingWorker(IRepository<Product, int> productRepository, IRepository<Device, long> deviceRepository, IDeviceDataApplicationService deviceDataApplicationService, ILogger<YuanGanWorker> logger, IHttpClientFactory httpClientFactory, IRepository<DeviceGroup, int> deviceGroupRepository, IRepository<DeviceGrouping> deviceGroupingRepository)
        {
            _productRepository = productRepository;
            _deviceRepository = deviceRepository;
            _deviceApplicationService = deviceDataApplicationService;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _deviceGroupRepository = deviceGroupRepository;
            _deviceGroupingRepository = deviceGroupingRepository;
        }

        private readonly IEventLoopGroup _bossGroup = new MultithreadEventLoopGroup();
        private readonly IEventLoopGroup _workerGroup = new MultithreadEventLoopGroup();

        private IChannel _boundChannel = default!;

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            InternalLoggerFactory.DefaultFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "[HH:mm:ss]";
            }));

            var bootstrap = new ServerBootstrap().Group(_bossGroup, _workerGroup).Channel<TcpServerSocketChannel>();

            bootstrap.Option(ChannelOption.SoBacklog, 100)
                .Handler(new LoggingHandler(DotNetty.Handlers.Logging.LogLevel.INFO))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new StringEncoder(Encoding.ASCII));
                    pipeline.AddLast(new IdleStateHandler(TimeSpan.FromMinutes(5), TimeSpan.Zero, TimeSpan.Zero));
                    pipeline.AddLast(new LoggingHandler(DotNetty.Handlers.Logging.LogLevel.INFO));
                    pipeline.AddLast(new StringDecoder(Encoding.ASCII));
                    pipeline.AddLast(new LineBasedFrameDecoder(1200));
                }));

            _boundChannel = await bootstrap.BindAsync(5212);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _boundChannel.CloseAsync();

            await Task.WhenAll(
                  _bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                  _workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));

            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
