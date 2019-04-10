using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public interface IJsonReader
    {
        event JsonConfigEventHandler ConfigReadFrom;

        object GetSetting(string settingName);
        T GetSetting<T>(string settingName);

        JObject ReadConfig(string pathToConfig);
    }
}
