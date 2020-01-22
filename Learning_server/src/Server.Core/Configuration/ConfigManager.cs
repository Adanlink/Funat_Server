using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Threading.Tasks;
using ChickenAPI.Core.Configurations;

namespace Server.Core.Configuration
{
    public static class ConfigManager
    {
        public static object LoadConfig(string path, IConfiguration configuration)
        {
            var exists = File.Exists(path);
            switch (Path.GetExtension(path))
            {
                case ".json":
                    if (exists)
                        return ConfigLoader.LoadJsonConfig(path, configuration.GetType());
                    else
                        return ConfigLoader.CreateJsonConfig(path, configuration);
                case ".xml":
                    if (exists)
                        return ConfigLoader.LoadXmlConfig(path, configuration.GetType());
                    else
                        return ConfigLoader.CreateXmlConfig(path, configuration);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
