using MG.Settings.JsonSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MG.Settings.JsonSettings
{
    public abstract class JsonConfig : IJsonConfig, IJsonMapper
    {
        #region CONSTANTS
        private const string SET_ACCESSOR = "set";

        #endregion

        #region PROPERTIES

        public abstract IDictionary DefaultSettings { get; }
        public abstract string[] SkipThese { get; }
        public abstract FileInfo ConfigFile { get; }
        public JsonSerializerSettings Serializer { get; set; }
        private Type ThisType { get; }
        private FieldInfo[] TheseFields { get; }
        public PropertyInfo[] TheseProps => this.GetType().GetProperties((BindingFlags)52).Where(
            x => !this.SkipThese.Contains(x.Name)).ToArray();

        #endregion

        #region EVENTS
        public event JsonConfigEventHandler ConfigReadFrom;
        public event JsonConfigEventHandler ConfigRemoved;
        public event JsonConfigEventHandler ConfigWrittenTo;
        public event PropertyMapperEventHandler PropertiesMapped;

        private void OnPropertiesMapped(PropertyMapperEventArgs e)
        {
            if (this.PropertiesMapped != null)
                this.PropertiesMapped(this, e);
        }
        private void OnPropertiesMapped(PropertyMappingType mapType)
        {
            this.OnPropertiesMapped(new PropertyMapperEventArgs(mapType, this));
        }

        #endregion

        #region CONSTRUCTORS
        public JsonConfig()
        {
            ThisType = this.GetType();
            TheseFields = ThisType.GetFields((BindingFlags)36);     // Instance & Non-Public
            Serializer = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }

        #endregion

        #region INHERITANCE METHODS
        public object GetSetting(string settingName)
        {
            throw new NotImplementedException();
        }

        public T GetSetting<T>(string settingName)
        {
            throw new NotImplementedException();
        }

        public string MakeNewConfig(string fullPath)
        {
            throw new NotImplementedException();
        }

        public JObject ReadConfig(string pathToConfig)
        {
            throw new NotImplementedException();
        }

        public void RemoveSetting(string settingName)
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public string ToJson(Formatting asFormat, bool useCamelCase) => 
            JsonConvert.SerializeObject(this, asFormat, this.Serializer);

        public bool WriteSetting(string settingName, object settingValue)
        {
            throw new NotImplementedException();
        }

        public bool WriteSetting<T>(string settingName, T settingValue)
        {
            throw new NotImplementedException();
        }

        protected abstract IJsonMapper Construct(string jsonStr);

        public void MatchToSet(string jsonStr)
        {
            var obj = this.Construct(jsonStr);

            this.CopyFrom(obj);
            this.OnPropertiesMapped(PropertyMappingType.MatchToInteralSet);
        }

        public object[] KeysToArray(IDictionary dict) =>
            dict.Keys.Cast<object>().ToArray();

        public void CopyFrom(IJsonMapper obj)
        {
            string[] keys = new string[obj.TheseProps.Length];
            for (int k = 0; k < obj.TheseProps.Length; k++)
            {
                keys[k] = obj.TheseProps[k].Name;
            }
            for (int i1 = 0; i1 < obj.TheseProps.Length; i1++)
            {
                var pi = obj.TheseProps[i1];
                var setMethod = this.GetInternalSet(pi);
                var valToSet = pi.GetValue(obj);

                for (int i2 = 0; i2 < keys.Length; i2++)
                {
                    var key = keys[i2];

                    if (pi.Name.Equals(key))
                    {
                        setMethod.Invoke(this, new object[1] { pi.GetValue(obj) });
                    }
                }
            }
        }

        #endregion

        #region STATIC METHODS
        public static T FromJObject<T>(JObject job)
            where T : IJsonConfig
        {
            var newT = Activator.CreateInstance<T>();
            string jsonContent = JsonConvert.SerializeObject(job);
            return JsonConvert.DeserializeAnonymousType(jsonContent, newT);
        }



        #endregion

        #region PRIVATE METHODS

        private MethodInfo GetInternalSet(PropertyInfo pi) =>
            pi.GetAccessors(true).Single(x => x.Name.StartsWith(SET_ACCESSOR, StringComparison.InvariantCulture));

        #endregion
    }
}
