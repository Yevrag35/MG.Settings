using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG.Settings.Json.Testing
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class MySettingManager : JsonSettingsManager
    {
        private string _comp;
        private int _cid;

        [JsonProperty("company")]
        public string Company
        {
            get => _comp;
            set
            {
                string oldVal = _comp.ToString();
                _comp = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, "Company", value, oldVal);
            }
        }
        
        [JsonProperty("companyId")]
        public int CompanyNumber
        {
            get => _cid;
            set
            {
                int oldVal = int.Parse(_cid.ToString());
                _cid = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, "CompanyNumber", value, oldVal);
            }
        }

        public MySettingManager() { }

        private void OnSettingsChanged(SettingChangedAction action)
        {
            base.OnSettingsChanged(new SettingsChangedEventArgs(action));
        }
        private void OnSettingsChanged(SettingChangedAction action, string propertyName,object newValue, object oldValue)
        {
            base.OnSettingsChanged(new SettingsChangedEventArgs(action, propertyName, newValue, oldValue));
        }
        private void OnSettingsChanged(SettingChangedAction action, IList newItems, IList oldItems)
        {
            base.OnSettingsChanged(new SettingsChangedEventArgs(action, newItems, oldItems));
        }
        private void OnSettingsChanged(SettingChangedAction action, IJsonSettings newSettings, IJsonSettings oldSettings)
        {
            base.OnSettingsChanged(new SettingsChangedEventArgs(action, newSettings, oldSettings));
        }
    }
}
