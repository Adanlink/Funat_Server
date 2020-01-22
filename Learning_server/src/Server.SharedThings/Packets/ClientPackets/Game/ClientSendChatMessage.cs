using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.SharedThings.Packets.ClientPackets.Game
{
    [PacketProperties(PacketType.ClientSendChatMessage)]
    [MessagePackObject]
    public class ClientSendChatMessage : IPacket
    {
        [Key(0)]
        public string Message { get; set; }
    }
}
