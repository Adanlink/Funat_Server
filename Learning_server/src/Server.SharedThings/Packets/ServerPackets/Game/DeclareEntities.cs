using System.Collections.Generic;
using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.Representations;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.DeclareEntities)]
    //[DataContract]
    [MessagePackObject]
    public class DeclareEntities : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public HashSet<BasicEntity> Entities { get; set; }
    }
}