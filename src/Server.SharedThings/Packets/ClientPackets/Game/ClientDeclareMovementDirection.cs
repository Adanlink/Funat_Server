using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Packets.ClientPackets.Enums;

namespace Server.SharedThings.Packets.ClientPackets.Game
{
    [PacketProperties(PacketType.MovementDirectionRequest)]
    //[DataContract]
    [MessagePackObject]
    public class ClientDeclareMovementDirection : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public MovementDirection MovementDirection { get; set; }
    }
}