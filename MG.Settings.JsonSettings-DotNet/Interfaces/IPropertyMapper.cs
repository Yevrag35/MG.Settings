using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MG.Settings.JsonSettings
{
    public interface IJsonMapper
    {
        event PropertyMapperEventHandler PropertiesMapped;

        string[] SkipThese { get; }
        PropertyInfo[] TheseProps { get; }

        void MatchToSet(string jsonStr);

        object[] KeysToArray(IDictionary dict);
    }
}
