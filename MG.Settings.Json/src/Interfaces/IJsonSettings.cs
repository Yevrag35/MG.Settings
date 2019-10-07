using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace MG.Settings.Json
{
    public interface IJsonSettings : IReadable, ISavable
    {
        event SettingsChangedEventHandler NotifySettingsChanged;

        Encoding Encoding { get; set; }
        string FilePath { get; set; }
        JsonSerializerSettings JsonSerializer { get; set; }
        void SetLoadSerializer(JsonSerializer loadSerializer);
        JObject SettingsAsJson { get; }
    }
}
