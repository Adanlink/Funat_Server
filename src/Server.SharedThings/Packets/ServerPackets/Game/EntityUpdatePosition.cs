﻿using System;
using System.Runtime.Serialization;
using MessagePack;

namespace Server.SharedThings.Packets.ServerPackets.Game
{
    [PacketProperties(PacketType.EntityUpdatePosition)]
    //[DataContract]
    [MessagePackObject]
    public class EntityUpdatePosition : IPacket
    {
        //[DataMember(Order = 0)]
        [Key(0)]
        public string Id { get; set; }
        
        //[DataMember(Order = 1)]
        [Key(1)]
        public float X { get; set; }
        
        //[DataMember(Order = 2)]
        [Key(2)]
        public float Y { get; set; }
    }
}