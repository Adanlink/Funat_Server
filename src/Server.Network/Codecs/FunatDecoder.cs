using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Network.Codecs
{
    public class FunatDecoder : MessageToMessageDecoder<IByteBuffer>
    {
        private static readonly Encoding Encoding = Encoding.GetEncoding(1256);

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var asd = Encoding.GetString(message.Array);
            var packets = asd.Split(new string[] { "Funat" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var packet in packets)
            {
                if (string.IsNullOrEmpty(packet))
                {
                    return;
                }
                output.Add(Unpooled.WrappedBuffer(Encoding.GetBytes(packet)));
            }
        }
    }
}