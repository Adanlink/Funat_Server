using System;
using Server.Database;
using ChickenAPI.Core.Configurations;

namespace Server.World.Configuration
{
    public class WorldConfiguration : IConfiguration
    {
        public ushort Port { get; set; } = 27451;

        public ushort MaxConnectionsPerIp { get; set; } = 4;

        public byte Tps { get; set; } = 16;

        /// <summary>
        /// ChunkSize x ChunkSize
        /// </summary>
        public ushort ChunkSize { get; set; } = 64;

        public DatabaseConfiguration DatabaseConfiguration { get; set; } = new DatabaseConfiguration();

        public CharacterConfiguration CharacterConfiguration { get; set; } = new CharacterConfiguration();
    }
}
