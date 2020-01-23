using System;
using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.RevokeEntity)]
    //[DataContract]
    [MessagePackObject]
    public class RevokeEntity : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Id { get; set; }
    }
}