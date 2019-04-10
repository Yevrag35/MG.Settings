using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;

namespace MG.Settings.JsonSettings
{
    public interface IJsonConfig : IJsonSaver
    {
        IDictionary DefaultSettings { get; }
        FileInfo ConfigFile { get; }
        JsonSerializerSettings Serializer { get; set; }

        string MakeNewConfig(string fullPath);

        string ToJson(Formatting asFormat, bool useCamelCase);
    }
}
