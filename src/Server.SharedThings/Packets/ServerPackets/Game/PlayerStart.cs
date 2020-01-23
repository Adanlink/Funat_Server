using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.Representations;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.PlayerStart)]
    //[DataContract]
    [MessagePackObject]
    public class PlayerStart : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public OwnPlayer OwnPlayer { get; set; }
    }
}