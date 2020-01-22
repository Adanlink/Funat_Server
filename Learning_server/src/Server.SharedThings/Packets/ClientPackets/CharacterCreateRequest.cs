using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.CharacterCreateRequest)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterCreateRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Nickname { get; set; }
    }
}