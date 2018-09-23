using MG.Attributes;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

namespace MG
{
    public sealed partial class AppSettings : AttributeResolver
    {
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
        private bool ValueExists(string valName) => _rk.GetValue(valName) != null;
    }
}