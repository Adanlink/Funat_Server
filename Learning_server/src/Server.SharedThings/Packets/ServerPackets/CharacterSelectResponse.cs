using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.CharacterSelectResponse)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterSelectResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public CharacterSelectResponseType CharacterSelectResponseType { get; set; }
    }
}