using MG.Attributes;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

namespace MG
{
    public sealed partial class AppSettings : AttributeResolver
    {
        public T GetProperty<T>(string propName)
        {
            if (ValueExists(propName))
            {
                try
                {
                    var result = Convert.ChangeType(Properties[propName], typeof(T));
                    return (T)result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + Environment.NewLine +
                        "Offending Value: " + Properties[propName], e);
                }

            }
            return default;
        }

        // SetProperties is only used for Constructors
        internal void SetProperties(IDictionary keyValuePairs)
        {
            _vals = new OrderedDictionary();
            string[] keys = keyValuePairs.Keys.Cast<string>().ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                if (!ValueExists(key))
                {
                    SetPropertyValue(key, keyValuePairs[key]);
                }
                else
                {
                    _vals.Add(key, _rk.GetValue(key));
                }
            }
        }

        public void SetPropertyValue(string propName, object propVal)
        {
            RegistryValueKind kind = Translate(propVal);
            _rk.SetValue(propName, propVal, kind);
            if (_vals.Contains(propName))
            {
                _vals[propName] = propVal;
            }
            else
            {
                _vals.Add(propName, propVal);
            }
        }
        public bool ValueExists(string valName) => _rk.GetValue(valName) != null;

        public bool DataExists(string valueName)
        {
            bool result = false;
            if (ValueExists(valueName))
            {
                var kind = _rk.GetValueKind(valueName);
                result = kind == RegistryValueKind.String ? 
                    !string.IsNullOrEmpty((string)_rk.GetValue(valueName)) : true;
            }
            return result;
        }
    }
}