using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;
using Server.SharedThings.Packets;

namespace Server.Network.Packets.Serializers
{
    public interface ISerializer
    {
        IByteBuffer Serialize(IPacket packet);

        IPacket Deserialize(IByteBuffer byteBuffer);
    }
}
