using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Settings.JsonSettings
{
    public class SettingsDictionary : ISettingsDictionary
    {
        private List<KeyValuePair<string, object>> _entries;

        private SettingsDictionary() => _entries = new List<KeyValuePair<string, object>>();

        public SettingsDictionary(IDictionary dictionaryBase)
            : this()
        {
            object[] keys = dictionaryBase.Keys.Cast<object>().ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string strKey = Convert.ToString(keys[i]);
                object value = dictionaryBase[strKey];
                _entries.Add(new KeyValuePair<string, object>(strKey, value));
            }
        }

        object ISettingsDictionary.this[string key]
        {
            get
            {
                object result = null;
                for (int i = 0; i < _entries.Count; i++)
                {
                    var entry = _entries[i];
                    if (entry.Key == key)
                    {
                        result = entry.Value;
                        break;
                    }
                }
                return result;
            }
            set
            {
                int? index = ((ISettingsDictionary)this).IndexOf(key);
                if (index.HasValue)
                {
                    _entries.RemoveAt(index.Value);
                    _entries.Add(new KeyValuePair<string, object>(key, value));
                    _entries.Sort(new SettingsComparer());
                }
            }
        }

        int ISettingsDictionary.Count => _entries.Count;

        int? ISettingsDictionary.IndexOf(string settingName)
        {
            int? result = null;
            for (int i = 0; i < _entries.Count; i++)
            {
                var kvp = _entries[i];
                if (kvp.Key == settingName)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        void ISettingsDictionary.Remove(string settingName)
        {
            for (int i = _entries.Count; i >= 0; i--)
            {
                var kvp = _entries[i];
                if (kvp.Key == settingName)
                {
                    _entries.Remove(kvp);
                    break;
                }
            }
        }

        #region LINQ OPERATIONS
        KeyValuePair<string, object> ISettingsDictionary.Find(Predicate<KeyValuePair<string, object>> match) => _entries.Find(match);
        object ISettingsDictionary.FindValue(Predicate<KeyValuePair<string, object>> predicate) => _entries.Find(predicate).Value;

        void ISettingsDictionary.ChangeAll(object newValue, Func<KeyValuePair<string, object>, bool> match)
        {
            var newList = new List<KeyValuePair<string, object>>();
            var changeThese = _entries.Where(match).ToArray();
            for (int i = _entries.Count - 1; i >= 0; i--)
            {
                var entry = _entries[i];
                if (changeThese.Contains(entry))
                {
                    var newEntry = new KeyValuePair<string, object>(entry.Key, newValue);
                    _entries.Remove(entry);
                    newList.Add(newEntry);
                }
            }
            _entries.AddRange(newList);
            _entries.Sort(new SettingsComparer());
        }

        #endregion

        #region TO DICTIONARIES

        Dictionary<string, object> ISettingsDictionary.ToDictionary()
        {
            var newDict = new Dictionary<string, object>(_entries.Count);
            for (int i = 0; i < _entries.Count; i++)
            {
                KeyValuePair<string, object> de = _entries[i];
                newDict.Add(de.Key, de.Value);
            }
            return newDict;
        }

        Dictionary<string, T> ISettingsDictionary.ToDictionary<T>()
        {
            var newDict = new Dictionary<string, T>(_entries.Count);
            for (int i = 0; i < _entries.Count; i++)
            {
                KeyValuePair<string, object> de = _entries[i];
                newDict.Add(Convert.ToString(de.Key), (T)de.Value);
            }
            return newDict;
        }

        #endregion

        public IEnumerator GetEnumerator() => _entries.GetEnumerator();
    }

    public class SettingsComparer : IComparer<KeyValuePair<string, object>>
    {
        public int Compare(KeyValuePair<string, object> x, KeyValuePair<string, object> y) =>
            x.Key.CompareTo(y.Key);
    }
}
