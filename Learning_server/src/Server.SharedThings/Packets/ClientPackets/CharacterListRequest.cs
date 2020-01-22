using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.CharacterListRequest)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterListRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public byte A { get; set; }
    }
}