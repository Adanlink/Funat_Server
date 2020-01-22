using System;

namespace Server.SharedThings.Packets
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class PacketPropertiesAttribute : Attribute
    {
        public uint Identifier { get; set; }

        //public DateTimeOffset TimeOfCraft {get; set;}

        public PacketPropertiesAttribute(uint identifier)
        {
            Identifier = identifier;
        }

        public PacketPropertiesAttribute(PacketType packetType)
        {
            Identifier = (uint)packetType;
        }
    }
}