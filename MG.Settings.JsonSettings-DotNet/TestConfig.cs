using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MG.Settings.JsonSettings
{
    public class TestConfig : JsonConfig
    {
        public override IDictionary DefaultSettings => new Dictionary<string, object>
        {
            { "heyThere", "whatup?" }
        };

        public override string[] SkipThese => new string[5]
        {
            "DefaultSettings", "SkipThese", "ConfigFile", "TheseProps", "Serializer"
        };

        public override FileInfo ConfigFile { get; }

        public string HeyThere { get; set; }

        public TestConfig() : base() { }


        public TestConfig(string pathToConfig)
            : base() => ConfigFile = new FileInfo(pathToConfig);

        protected override IJsonMapper Construct(string jsonStr)
        {
            return JsonConvert.DeserializeAnonymousType(jsonStr, this, this.Serializer);
        }
    }
}
