using System;
using Server.Database;
using ChickenAPI.Core.Configurations;
using Server.Crypto;

namespace Server.Login
{
    public class LoginConfiguration : IConfiguration
    {
        public ushort Port { get; set; } = 27450;

        public DatabaseConfiguration DatabaseConfiguration { get; set; } = new DatabaseConfiguration();

        public HasherConfiguration HasherConfiguration { get; set; } = new HasherConfiguration();
    }
}
