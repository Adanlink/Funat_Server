using System;
using System.Runtime.Serialization;
using MessagePack;
using Server.SharedThings.Enums;

namespace Server.SharedThings.Packets.Representations
{
    //[DataContract]
    [MessagePackObject]
    public class Character
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Nickname { get; set; }
        
        //[DataMember(Order = 1)]
        [Key(1)]
        public AuthorityType Authority { get; set; }
        
        //[DataMember(Order = 2)]
        [Key(2)]
        public DateTime TimeOfCreation { get; set; }
        
        //[DataMember(Order = 3)]
        [Key(3)]
        public DateTime LastTimePlayed { get; set; }
        
        //[DataMember(Order = 4)]
        [Key(4)]
        public long MapId { get; set; }
    }
}