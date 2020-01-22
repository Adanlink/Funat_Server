//Based from ChickenAPI.Game.Impl made by Blowa
using System;
using ChickenAPI.Core.Configurations;
using SharpYaml.Serialization;
using System.Threading.Tasks;

namespace Server.Core.Configuration
{
    public class YamlConfigurationSerializer : IConfigurationSerializer
    {
        private readonly static Serializer serializer = new Serializer();

        public string Serialize<T>(T conf) where T : IConfiguration => serializer.Serialize(conf);

        public T Deserialize<T>(string buffer) where T : IConfiguration
        {
            //Task.Run(() => .(buffer));
            return serializer.Deserialize<T>(buffer);
        }
    }
}
