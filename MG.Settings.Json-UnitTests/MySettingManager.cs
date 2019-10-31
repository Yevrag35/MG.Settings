using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MG.Settings.Json.Tests
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class MySettingManager : JsonSettingsManager
    {
        private string _comp;
        private Guid _cuid;
        private uint _cid;

        [JsonProperty("company")]
        public string Company
        {
            get => _comp;
            set
            {
                string oldVal = null;
                if (!string.IsNullOrEmpty(_comp))
                    oldVal = _comp.ToString();

                _comp = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, "Company", value, oldVal);
            }
        }
        
        [JsonProperty("companyId")]
        public uint CompanyNumber
        {
            get => _cid;
            set
            {
                int oldVal = int.Parse(_cid.ToString());
                _cid = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, "CompanyNumber", value, oldVal);
            }
        }

        [JsonProperty("companyUniqueId")]
        public Guid CompanyId
        {
            get => _cuid;
            set
            {
                var oldId = _cuid.ToString();
                var newId = value.ToString();
                _cuid = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, "CompanyId", newId, oldId);
            }
        }

        [JsonProperty("customers")]
        private IList<Customer> _customers;

        public IList<Customer> Customers
        {
            get => _customers;
            set
            {
                var oldCustomers = new List<Customer>(_customers);
                _customers = value;
                this.OnSettingsChanged(SettingChangedAction.Replace, (IList)value, oldCustomers);
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
