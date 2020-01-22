using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.RegisterRequest)]
    //[DataContract]
    [MessagePackObject]
    public class RegisterRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Key { get; set; }

        //[DataMember(Order = 1)]
        [Key(1)]
        public string Username { get; set; }

        //[DataMember(Order = 2)]
        [Key(2)]
        public byte[] PasswordHash { get; set; }
    }
}
