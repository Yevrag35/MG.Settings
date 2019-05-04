using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MG.Settings.JsonSettings
{
    public class SettingsManager : IJsonReader, IJsonRemover, IJsonSaver, IJsonWriter
    {
        private static readonly JsonSerializerSettings serializer = new JsonSerializerSettings
        {
            //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include
        };

        private ISettingsDictionary _iset;
        private bool _even = false;

        #region EVENTS

        public event JsonConfigEventHandler ConfigLoaded;
        public event JsonConfigEventHandler ConfigReadFrom;
        public event JsonConfigEventHandler ConfigReloaded;
        public event JsonConfigEventHandler ConfigRemovedFrom;
        public event JsonConfigEventHandler ConfigWrittenTo;
        public event JsonConfigEventHandler ConfigSaved;

        private void OnConfigReadFrom()
        {
            if (this.ConfigReadFrom != null)
                this.ConfigReadFrom(this, new JsonConfigEventArgs(JsonConfigChangedAction.Read));
        }
        private void OnConfigReadFrom(string settingName, object settingValue)
        {
            if (this.ConfigReadFrom != null)
                this.ConfigReadFrom(this, new JsonConfigEventArgs(JsonConfigChangedAction.Read, settingName, settingValue));
        }
        private void OnConfigReloaded()
        {
            if (this.ConfigReloaded != null)
                this.ConfigReloaded(this, new JsonConfigEventArgs(JsonConfigChangedAction.Reloaded));
        }
        private void OnConfigLoaded()
        {
            if (this.ConfigLoaded != null)
                this.ConfigLoaded(this, new JsonConfigEventArgs(JsonConfigChangedAction.Loaded));
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
            if (this.ConfigSaved != null)
                this.ConfigSaved(this, new JsonConfigEventArgs(JsonConfigChangedAction.Save));
        }

        #endregion

        public bool EnableRaisingEvents
        {
            get => _even;
            set => _even = value;
        }
        public ISettingsDictionary Settings => _iset;
        public string Path { get; private set; }

        //public SettingsManager() { }

        public SettingsManager(string pathToConfig) => this.ReadConfig(pathToConfig);

        private T Cast<T>(dynamic o) => o != null
            ? (T)o
            : default;

        public object GetSetting(string setting)
        {
            object retVal = _iset[setting];
            if (_even)
                this.OnConfigReadFrom();
            return retVal;
        }
        public T GetSetting<T>(string setting)
        {
            object value = _iset[setting];
            if (_even)
                this.OnConfigReadFrom();
            return Cast<T>(value);
        }      

        public void ReadConfig(string pathToConfig)
        {
            if (File.Exists(pathToConfig))
            {
                this.Path = pathToConfig;
                string allText = File.ReadAllText(pathToConfig);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(allText, serializer);
                _iset = SettingsDictionary.NewSettingsDictionary(dict);
                if (_even)
                    this.OnConfigLoaded();
            }
        }

        public void Reload()
        {
            if (File.Exists(this.Path))
            {
                string allText = File.ReadAllText(this.Path);
                var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(allText, serializer);
                _iset.Reload(dic);
                if (_even)
                    this.OnConfigReloaded();
            }
        }

        public void RemoveSetting(string setting)
        {
            object origObj = _iset[setting];
            _iset.Remove(setting);
            if (_even)
                this.OnConfigAddRemove(JsonConfigChangedAction.Remove, setting, origObj);
        }

        public void SaveConfig()
        {
            var dict = _iset.ToDictionary();
            string jsonStr = JsonConvert.SerializeObject(dict, serializer);
            File.WriteAllText(this.Path, jsonStr);

            if (_even)
                this.OnConfigSaved();
        }

        public void WriteSetting(string setting, object value)
        {
            _iset[setting] = value;
            if (_even)
                this.OnConfigAddRemove(JsonConfigChangedAction.Add, setting, value);
        }
    }
}
