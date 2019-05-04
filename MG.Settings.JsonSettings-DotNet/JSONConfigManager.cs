using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MG.Settings.JsonSettings
{
    public partial class JSONConfigManager : IJsonReader, IJsonRemover, IJsonWriter, IJsonSaver, IEnumerable
    {
        public event JsonConfigEventHandler ConfigReadFrom;
        public event JsonConfigEventHandler ConfigChanged;
        public event JsonConfigEventHandler ConfigRemovedFrom;
        public event JsonConfigEventHandler ConfigWrittenTo;

        private JObject _job;

        public string ConfigFilePath { get; private set; }
        //public int? Count => _job != null ? _job.Count : (int?)null;

        //public ISettingsDictionary AppSettings { get; private set; }

        public JSONConfigManager() { }

        public JSONConfigManager(string pathToConfig) => this.ReadConfig(pathToConfig);

        public IEnumerator GetEnumerator() => _job.GetEnumerator();

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

            else if (action == JsonConfigChangedAction.Remove && this.ConfigRemovedFrom != null)
                this.ConfigRemovedFrom(this, new JsonConfigEventArgs(action, name, value));
        }
        private void OnConfigSaved()
        {
            if (this.ConfigChanged != null)
                this.ConfigChanged(this, new JsonConfigEventArgs(JsonConfigChangedAction.Save));
        }

        #endregion

        //public object GetSetting(string settingName) => this.GetSetting<object>(settingName);

        public T GetSetting<T>(string settingName) => _job[settingName].Value<T>();

        public T GetSettingFromPath<T>(string settingPath) => _job.SelectToken(settingPath).Value<T>();

        JObject IJsonReader.ReadConfig(string pathToConfig) => ReadFromConfig<JObject>(pathToConfig);

        public void ReadConfig(string pathToConfig)
        {
            string jsonStr = File.ReadAllText(pathToConfig);
            _job = JsonConvert.DeserializeObject<JObject>(jsonStr);
            ConfigFilePath = pathToConfig;

            //var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
            //this.AppSettings = new SettingsDictionary(dict);
            this.OnConfigReadFrom();
        }

        public void SaveConfig()
        {
            if (_job == null)
                throw new InvalidOperationException("You must read a config file before you can save!");

            var serializer = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            string backToStr = JsonConvert.SerializeObject(_job, Formatting.Indented, serializer);
            File.WriteAllText(ConfigFilePath, backToStr);
            this.OnConfigSaved();
        }

        public void RemoveSetting(string settingName)
        {
            JToken val = _job[settingName];
            val.Remove();
            this.OnConfigAddRemove(JsonConfigChangedAction.Remove, settingName, val);
        }
        public void RemoveSettingByPath(string settingPath)
        {
            JToken val = _job.SelectToken(settingPath);
            val.Remove();
            this.OnConfigAddRemove(JsonConfigChangedAction.Remove, settingPath, val);
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

        internal JObject GetUnderlyingObject() => _job;
    }
}
