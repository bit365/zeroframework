using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class DictionaryHandler : SimpleChannelInboundHandler<Dictionary<string, string>>
    {
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
    }
}
