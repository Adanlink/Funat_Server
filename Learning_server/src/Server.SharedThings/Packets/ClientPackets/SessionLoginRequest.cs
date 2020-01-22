using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ClientPackets
{
    [PacketProperties(PacketType.SessionLoginRequest)]
    //[DataContract]
    [MessagePackObject]
    public class SessionLoginRequest : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public uint GameVersion { get; set; }

        //[DataMember(Order = 1)]
        [Key(1)]
        public string Username { get; set; }

        //TODO string to Guid
        //[DataMember(Order = 2)]
        [Key(2)]
        public string SessionId { get; set; }
    }
}
