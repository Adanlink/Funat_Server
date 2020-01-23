using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ServerPackets.Enums;

namespace Server.SharedThings.Packets.ServerPackets
{
    [PacketProperties(PacketType.RegisterResponse)]
    //[DataContract]
    [MessagePackObject]
    public class RegisterResponse : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public RegisterResponseType RegisterResponseType { get; set; }
    }
}
