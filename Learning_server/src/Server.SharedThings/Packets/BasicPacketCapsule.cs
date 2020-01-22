using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets
{
    //[DataContract]
    [MessagePackObject]
    public class BasicPacketCapsule
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public uint Identifier { get; set; }

        //[DataMember(Order = 1)]
        [Key(1)]
        public byte[] Packet { get; set; }
    }
}
