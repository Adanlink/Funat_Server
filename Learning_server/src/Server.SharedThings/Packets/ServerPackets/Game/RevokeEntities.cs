using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.RevokeEntities)]
    //[DataContract]
    [MessagePackObject]
    public class RevokeEntities
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public HashSet<string> Ids { get; set; }
    }
}