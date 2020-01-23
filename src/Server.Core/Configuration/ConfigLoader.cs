using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Threading.Tasks;
using ChickenAPI.Core.Configurations;

namespace Server.Core.Configuration
{
    internal static class ConfigLoader
    {
        public static object LoadJsonConfig(string path, Type configuration)
        {
            return JsonConvert.DeserializeObject(
                File.ReadAllText(path), configuration);
        }

        public static IConfiguration CreateJsonConfig(string path, IConfiguration configuration)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(configuration));
            return configuration;
        }

        public static object LoadXmlConfig(string path, Type configuration)
        {
            var xmlFile = new XmlDocument();
            xmlFile.LoadXml(File.ReadAllText(path));
            return JsonConvert.DeserializeObject(
                JsonConvert.SerializeXmlNode(xmlFile), configuration);
        }

        public static IConfiguration CreateXmlConfig(string path, IConfiguration configuration)
        {
            File.WriteAllText(path, JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(configuration)).ToString());
            return configuration;
        }
    }
}
