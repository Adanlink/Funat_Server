using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.Representations;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.DeclareEntity)]
    //[DataContract]
    [MessagePackObject]
    public class DeclareEntity : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public Player EntityBase { get; set; }
    }
}