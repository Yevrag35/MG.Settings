using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MG.Settings.JsonSettings
{
    public class ConfigEquality : IEqualityComparer<JSONConfigManager>
    {
        public bool Equals(JSONConfigManager x, JSONConfigManager y)
        {
            
        }
        public int GetHashCode(JSONConfigManager obj) => 0;
    }
}
