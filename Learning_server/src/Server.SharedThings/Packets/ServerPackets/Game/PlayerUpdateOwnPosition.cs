using System;
using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.PlayerUpdateOwnPosition)]
    //[DataContract]
    [MessagePackObject]
    public class PlayerUpdateOwnPosition : IPacket
    {
        //[DataMember(Order = 1)]
        [Key(0)]
        public float X { get; set; }

        //[DataMember(Order = 2)]
        [Key(1)]
        public float Y { get; set; }
    }
}