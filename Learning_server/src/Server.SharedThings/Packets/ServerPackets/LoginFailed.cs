using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.LoginFailed)]
    //[DataContract]
    [MessagePackObject]
    public class LoginFailed : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public LoginFailedType LoginFailedType { get; set; }
    }
}