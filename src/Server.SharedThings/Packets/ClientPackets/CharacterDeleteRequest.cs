using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.CharacterDeleteRequest)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterDeleteRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Nickname { get; set; }
    }
}