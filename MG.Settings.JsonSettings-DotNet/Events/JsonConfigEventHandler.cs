using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public delegate void JsonConfigEventHandler(object sender, JsonConfigEventArgs e);

    public class JsonConfigEventArgs : EventArgs
    {
        public JsonConfigChangedActions Action { get; }
        public IJsonConfig JsonConfig { get; }

        public JsonConfigEventArgs(JsonConfigChangedActions action, IJsonConfig config)
        {
            Action = action;
            JsonConfig = config;
        }
    }

    public enum JsonConfigChangedActions
    {
        Add = 0,
        Removed = 1,
        Update = 2
    }
}
