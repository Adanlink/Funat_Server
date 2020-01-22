using System.Collections.Generic;
using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.Representations;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.CharacterListResponse)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterListResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public List<Character> Characters { get; set; }
    }
}