using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.Representations
{
    //[DataContract]
    [MessagePackObject]
    public class OwnPlayer
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Nickname { get; set; }

        //[DataMember(Order = 1)]
        [Key(1)]
        public float X { get; set; }
        
        //[DataMember(Order = 2)]
        [Key(2)]
        public float Y { get; set; }
    }
}