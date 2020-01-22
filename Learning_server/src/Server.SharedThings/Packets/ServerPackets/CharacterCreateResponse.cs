using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.CharacterCreateResponse)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterCreateResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public CharacterCreateResponseType CharacterCreateResponseType { get; set; }
    }
}