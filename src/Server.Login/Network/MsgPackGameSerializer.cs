using System;
using System.Reflection;
using ChickenAPI.Core.Logging;
using DotNetty.Buffers;
using MessagePack;
using Server.Network.Packets.Serializers;
using Server.SharedThings.Packets;

namespace Server.Login.Network
{
    public class MsgPackGameSerializer : ISerializer
    {
        private readonly ILogger Log;
        private readonly IPacketFactory packetFactory;

        public MsgPackGameSerializer(ILogger log, IPacketFactory _packetFactory)
        {
            packetFactory = _packetFactory;
            Log = log;
        }

        public IPacket Deserialize(IByteBuffer byteBuffer)
        {
            try
            {
                var packet = MessagePackSerializer.Deserialize<BasicPacketCapsule>(byteBuffer.GetIoBuffer().ToArray());
                var packetType = packetFactory.GetPacketById(packet.Identifier);

                if (packetType == null)
                {
                    Log.Warn($"Packet with identifier: [{packet.Identifier}] couldn't be found.");
                    return null;
                }

                return MessagePackSerializer.Deserialize(packetType, packet.Packet) as IPacket;
            }
            catch (Exception e)
            {
                Log.Error("[DESERIALIZE]", e);
                return null;
            }
        }

        /*private uint GetHeader(IByteBuffer content)
        {
            var header = content.ReadInt();
            content.DiscardReadBytes();
            return (uint)header;
        }*/

        public IByteBuffer Serialize(IPacket packet)
        {
            try
            {
                var buffer = new UnpooledByteBufferAllocator().Buffer();

                buffer.WriteBytes(
                    MessagePackSerializer.Serialize(new BasicPacketCapsule()
                {
                    Identifier = packet.GetType().GetCustomAttribute<PacketPropertiesAttribute>().Identifier,
                    Packet = MessagePackSerializer.Serialize(packet.GetType(), packet)
                }));

                /*buffer.WriteInt((int)packet.GetType().GetCustomAttribute<PacketPropertiesAttribute>().Identifier);
                buffer.WriteBytes(MessagePackSerializer.Typeless.Serialize(packet));*/

                return buffer;
            }
            catch (Exception e)
            {
                Log.Error("[SERIALIZE]", e);
                return null;
            }
        }
    }
}
