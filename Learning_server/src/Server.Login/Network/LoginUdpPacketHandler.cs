using System;
using System.Net;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using MessagePack;
using Server.Network.Packets;

namespace Server.Login.Network
{
    internal class LoginUdpPacketHandler : SimpleChannelInboundHandler<DatagramPacket>
    {
        private readonly IPacketFactory packetFactory;

        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket msg)
        {
            HandlePacket(msg.Sender, msg.Content);
        }

        public LoginUdpPacketHandler(IPacketFactory _packetFactory)
        {
            packetFactory = _packetFactory;
        }

        public void HandlePacket(EndPoint sender, IByteBuffer content)
        {
            Console.WriteLine(content.GetString(content.ArrayOffset, content.ReadableBytes, Encoding.UTF8));
            try
            {
                var identificator = GetHeader(content);
                var DeserializedPacket = MessagePackSerializer.Deserialize(packetFactory.GetPacketById(identificator), content.GetIoBuffer().ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private uint GetHeader(IByteBuffer content)
        {
            var header = content.ReadByte();
            content.DiscardReadBytes();
            return header;
        }

        private string PrintByteArray(byte[] byteArray)
        {
            var sb = new StringBuilder("new byte[] { ");
            for (var i = 0; i < byteArray.Length; i++)
            {
                var b = byteArray[i];
                sb.Append(b);
                if (i < byteArray.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
