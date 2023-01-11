using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;
using System;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class DictionaryHandler : SimpleChannelInboundHandler<Dictionary<string, string>>
    {
        static readonly IInternalLogger Logger = InternalLoggerFactory.GetInstance<DictionaryHandler>();

        private readonly IServiceProvider _serviceProvider;

        public DictionaryHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Dictionary<string, string> msg)
        {
            ctx.Channel.EventLoop.Execute(async () =>
            {
                try
                {
                    var _dictionaryDataService = _serviceProvider.GetRequiredService<IDictionaryDataService>();
                    await _dictionaryDataService.HandlingAaync(msg);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            });
        }
    }
}
