using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class StringToDictionaryDecoder : MessageToMessageDecoder<string>
    {
        protected override void Decode(IChannelHandlerContext context, string message, List<object> output)
        {
            message = message[6..^8].Replace("CP=&&", string.Empty);

            char[] separators = new char[] { ',', ';' };

            string[] subs = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            var keyValuePairs = subs.ToDictionary(s => s.Split('=').First(), s => s.Split('=').Last());

            output.Add(keyValuePairs);
        }
    }
}
