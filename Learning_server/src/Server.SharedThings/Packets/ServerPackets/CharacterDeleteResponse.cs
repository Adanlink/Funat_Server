using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.CharacterDeleteResponse)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterDeleteResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public CharacterDeleteResponseType CharacterDeleteResponseType { get; set; }
    }
}