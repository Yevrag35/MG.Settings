using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Settings.JsonSettings
{
    public interface ISettingsDictionary : IEnumerable
    {
        object this[string key] { get; set; }

        int Count { get; }

        void ChangeAll(object newValue, Func<KeyValuePair<string, object>, bool> match);

        int? IndexOf(string settingName);

        KeyValuePair<string, object> Find(Predicate<KeyValuePair<string, object>> match);
        object FindValue(Predicate<KeyValuePair<string, object>> match);

        void Remove(string settingName);

        Dictionary<string, object> ToDictionary();
        Dictionary<string, T> ToDictionary<T>();
    }
}
