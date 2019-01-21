using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public delegate void JsonConfigEventHandler(object sender, JsonConfigEventArgs e);

    public class JsonConfigEventArgs : EventArgs
    {
        public JsonConfigChangedAction Action { get; }
        public IJsonSaver OldWrittenConfig { get; }
        public IJsonSaver NewWrittenConfig { get; }
        public string SettingName { get; }
        public object SettingValue { get; }

        public JsonConfigEventArgs(JsonConfigChangedAction action) =>
            Action = action;

        public JsonConfigEventArgs(JsonConfigChangedAction action, string settingName, object settingValue)
            : this(action)
        {
            SettingName = settingName;
            SettingValue = settingValue;
        }

    }

    public enum JsonConfigChangedAction
    {
        Add = 0,
        Remove = 1,
        Update = 2,
        Save = 3,
        Read = 4
    }
}
