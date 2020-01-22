using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.SessionLoginResponse)]
    //[DataContract]
    [MessagePackObject]
    public class SessionLoginResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public SessionLoginResponseType SessionLoginResponseType { get; set; }
    }
}
