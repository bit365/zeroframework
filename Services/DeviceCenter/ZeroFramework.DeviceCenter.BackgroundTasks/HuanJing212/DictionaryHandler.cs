using DotNetty.Common.Internal.Logging;
using DotNetty.Transport.Channels;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class DictionaryHandler : SimpleChannelInboundHandler<Dictionary<string, string>>
    {
        static readonly IInternalLogger Logger = InternalLoggerFactory.GetInstance<DictionaryHandler>();


        private readonly IDictionaryDataService _dictionaryDataService;

        public DictionaryHandler(IDictionaryDataService dictionaryDataService)
        {
            _dictionaryDataService = dictionaryDataService;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, Dictionary<string, string> msg)
        {
            ctx.Channel.EventLoop.Execute(async () =>
            {
                await _dictionaryDataService.HandlingAaync(msg);
            });
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Logger.Error(exception);
        }
    }
}
