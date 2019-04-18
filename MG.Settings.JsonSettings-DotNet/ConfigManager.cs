using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace MG.Settings.JsonSettings
{
    public partial class ConfigManager : IJsonReader, IJsonRemover, IJsonWriter, IJsonSaver
    {
        public event JsonConfigEventHandler ConfigReadFrom;
        public event JsonConfigEventHandler ConfigChanged;
        public event JsonConfigEventHandler ConfigRemoved;
        public event JsonConfigEventHandler ConfigWrittenTo;

        private JObject _job;

        public string ConfigFilePath { get; private set; }
        public int? Count => _job != null ? _job.Count : (int?)null;
        public bool? HasSettings => _job != null ? _job.HasValues : (bool?)null;

        public Dictionary<string, object> AppSettings { get; private set; }

        public ConfigManager() { }

        public ConfigManager(string pathToConfig) => this.ReadConfig(pathToConfig);

        #region EVENTS
        private void OnConfigReadFrom()
        {
            if (this.ConfigReadFrom != null)
                this.ConfigReadFrom(this, new JsonConfigEventArgs(JsonConfigChangedAction.Read));
        }
        private void OnConfigAddRemove(JsonConfigChangedAction action, string name, object value)
        {
            if (action == JsonConfigChangedAction.Add && this.ConfigWrittenTo != null)
                this.ConfigWrittenTo(this, new JsonConfigEventArgs(action, name, value));

            else if (action == JsonConfigChangedAction.Remove && this.ConfigRemoved != null)
                this.ConfigRemoved(this, new JsonConfigEventArgs(action, name, value));
        }
        private void OnConfigSaved()
        {
            if (this.ConfigChanged != null)
                this.ConfigChanged(this, new JsonConfigEventArgs(JsonConfigChangedAction.Save));
        }

        #endregion

        //public object GetSetting(string settingName) => this.GetSetting<object>(settingName);

        public T GetSetting<T>(string settingName) => (T)this.AppSettings[settingName];

        JObject IJsonReader.ReadConfig(string pathToConfig)
        {
            return null;
        }

        public void ReadConfig(string pathToConfig)
        {
            string jsonStr = File.ReadAllText(pathToConfig);
            //_job = JsonConvert.DeserializeObject<JObject>(jsonStr);
            ConfigFilePath = pathToConfig;
            this.OnConfigReadFrom();

            this.AppSettings = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
        }

        public void SaveConfig()
        {
            if (this.AppSettings == null)
                throw new InvalidOperationException("You must read a config file before you can save!");

            var serializer = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            string backToStr = JsonConvert.SerializeObject(this.AppSettings, Formatting.Indented, serializer);
            File.WriteAllText(ConfigFilePath, backToStr);
            this.OnConfigSaved();
        }

        public void RemoveSetting(string settingName)
        {
            //object value = this.AppSettings[settingName];
            //.Remove(settingName);
            this.OnConfigAddRemove(JsonConfigChangedAction.Remove, settingName, null);
        }

        public bool WriteSetting(string settingName, object settingValue)
        {
            bool result = false;
            try
            {
                var jtok = JToken.FromObject(settingValue);
                _job.Add(settingName, jtok);
                this.OnConfigAddRemove(JsonConfigChangedAction.Add, settingName, settingValue);
                result = true;
            }
            catch
            {
            }

            return result;
        }
    }
}
