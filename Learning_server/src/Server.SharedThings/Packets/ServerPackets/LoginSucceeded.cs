using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.LoginSucceeded)]
    //[DataContract]
    [MessagePackObject]
    public class LoginSucceeded : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string SessionId { get; set; }
    }
}