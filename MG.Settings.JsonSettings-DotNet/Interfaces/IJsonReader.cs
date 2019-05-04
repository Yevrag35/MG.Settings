using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public interface IJsonReader
    {
        event JsonConfigEventHandler ConfigLoaded;
        event JsonConfigEventHandler ConfigReadFrom;
        event JsonConfigEventHandler ConfigReloaded;

        object GetSetting(string settingName);
        T GetSetting<T>(string settingName);

        void ReadConfig(string pathToConfig);
        void Reload();
    }
}
