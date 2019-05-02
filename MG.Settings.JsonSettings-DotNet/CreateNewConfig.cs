using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;

namespace MG.Settings.JsonSettings
{
    public partial class JSONConfigManager
    {
        public static JSONConfigManager NewConfig(string pathToNewConfig, JContainer defaultSettings)
        {
            var serializer = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };
            return NewConfig(pathToNewConfig, defaultSettings, serializer);
        }

        public static JSONConfigManager NewConfig(string pathToNewConfig, JContainer defaultSettings, JsonSerializerSettings serializerSettings)
        {
            string jsonString = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented, serializerSettings);
            File.WriteAllText(pathToNewConfig, jsonString);

            JSONConfigManager configMan = null;
            if (File.Exists(pathToNewConfig))
                configMan = new JSONConfigManager(pathToNewConfig);

            return configMan;
        }

        public static JSONConfigManager NewConfig(string pathToNewConfig, IDictionary defaultSettings)
        {
            var serializer = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };
            string jsonString = JsonConvert.SerializeObject(defaultSettings, Formatting.Indented, serializer);
            File.WriteAllText(pathToNewConfig, jsonString);

            JSONConfigManager configMan = null;
            if (File.Exists(pathToNewConfig))
                configMan = new JSONConfigManager(pathToNewConfig);

            return configMan;
        }

        public static T ReadFromConfig<T>(string jsonPath) where T : JContainer
        {
            string jsonString = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
