using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Server.Network.Codecs
{
    public class FunatEncoder : MessageToMessageEncoder<IByteBuffer>
    {
        private static readonly Encoding Encoding = Encoding.GetEncoding(1256);

        private static readonly List<byte> Separator = Encoding.GetBytes("Funat").ToList();

        protected override void Encode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            var asd = new List<byte>(Separator);
            var asd2 = Encoding.GetString(message.Array);
            asd.AddRange(Encoding.GetBytes(asd2.Replace("Funat", "*****", StringComparison.Ordinal)));
            //asd.AddRange(message.Array);
            output.Add(Unpooled.WrappedBuffer(asd.ToArray()));
        }
    }
}