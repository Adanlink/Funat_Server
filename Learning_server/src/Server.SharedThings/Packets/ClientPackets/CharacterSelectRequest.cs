using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.CharacterSelectRequest)]
    //[DataContract]
    [MessagePackObject]
    public class CharacterSelectRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Nickname { get; set; }
    }
}