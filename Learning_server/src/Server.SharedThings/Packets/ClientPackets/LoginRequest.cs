using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.LoginRequest)]
    [MessagePackObject]
    //[DataContract]
    public class LoginRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public uint GameVersion { get; set; }

        //[DataMember(Order = 1)]
        [Key(1)]
        public string Username { get; set; }

        //[DataMember(Order = 2)]
        [Key(2)]
        public byte[] PasswordHash { get; set; }
    }
}
