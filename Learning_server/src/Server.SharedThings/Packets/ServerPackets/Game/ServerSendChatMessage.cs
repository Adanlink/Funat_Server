using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.ServerSendChatMessage)]
    [MessagePackObject]
    public class ServerSendChatMessage : IPacket
    {
        [Key(0)]
        public string Message { get; set; }
    }
}
