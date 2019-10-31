using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MG.Settings.Json
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonSettingsManager : IJsonSettings
    {
        #region PRIVATE FIELDS/CONSTANTS

        private const string JSON_EXT = ".json";

        private Encoding _enc;
        private string _filePath;
        protected JObject InnerJob;
        //private JObject _job;
        private JsonSerializerSettings _js;
        private JsonSerializer _forLoading;

        #endregion

        #region PROPERTIES
        public Encoding Encoding
        {
            get => _enc;
            set => _enc = value ?? throw new ArgumentNullException("Encoding");
        }
        public bool Exists => !string.IsNullOrEmpty(_filePath) && File.Exists(_filePath);
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("FilePath");

                if (!Path.GetExtension(value).Equals(JSON_EXT))
                    throw new ArgumentException(string.Format("\"{0}\" does not end in the JSON extension.", value));

                _filePath = value;
            }
        }
        public JsonSerializerSettings JsonSerializer { get; set; }
        JObject IJsonSettings.SettingsAsJson
        {
            get
            {
                if (_forLoading == null)
                {
                    _forLoading = new JsonSerializer
                    {
                        NullValueHandling = NullValueHandling.Include,
                        Formatting = Formatting.Indented
                    };
                    _forLoading.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                }
                return JObject.FromObject(this, _forLoading);
            }
        }
        public event SettingsChangedEventHandler NotifySettingsChanged;

        #endregion

        public JsonSettingsManager()
        {
            _enc = Encoding.UTF8;
            this.InnerJob = new JObject();
        }
        public JsonSettingsManager(string filePath) : this() => this.FilePath = filePath;

        #region PUBLIC METHODS
        public static T ReadFromFileAs<T>(string filePath, Encoding encoding, JsonSerializerSettings serializer)
            where T : IJsonSettings
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file was not found.", filePath);

            string rawJson = File.ReadAllText(filePath, encoding);
            return serializer != null
                ? JsonConvert.DeserializeObject<T>(rawJson, serializer)
                : JsonConvert.DeserializeObject<T>(rawJson);
        }
        public void Read()
        {
            if (string.IsNullOrEmpty(_filePath))
                throw new ArgumentNullException("FilePath");

            string rawJson = File.ReadAllText(_filePath, _enc);

            if (this.JsonSerializer != null)
                JsonConvert.PopulateObject(rawJson, this, this.JsonSerializer);
            
            else
                JsonConvert.PopulateObject(rawJson, this);
        }
        //public abstract void Save();
        public virtual void Save()
        {
            using (var streamWriter = new StreamWriter(this.FilePath))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter)
                {
                    AutoCompleteOnClose = true,
                    CloseOutput = true,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    Formatting = Formatting.Indented,
                    Indentation = 1,
                    IndentChar = char.Parse("\t")
                })
                {
                    ((IJsonSettings)this).Save(jsonWriter);
                }
            }
        }
        void ISavable.Save(JsonTextWriter jsonWriter)
        {
            var converter = new StringEnumConverter(new CamelCaseNamingStrategy());
            ((IJsonSettings)this).SettingsAsJson.WriteTo(jsonWriter, converter);
        }
        //public void Save()
        //{
        //    if (string.IsNullOrEmpty(_filePath))
        //        throw new ArgumentNullException("FilePath");

        //    if (this.NotifySettingsChanged != null && this.Exists)
        //    {
        //        string raw = File.ReadAllText(_filePath, this.Encoding);
        //        var newThis = ReadFromFileAs<>
        //    }

        //    string jsonString = this.JsonSerializer != null
        //        ? JsonConvert.SerializeObject(this, this.JsonSerializer)
        //        : JsonConvert.SerializeObject(this);

        //    File.WriteAllText(_filePath, jsonString, this.Encoding);

        //}
        void IJsonSettings.SetLoadSerializer(JsonSerializer loadSerializer) => _forLoading = loadSerializer;

        #endregion

        #region PRIVATE METHODS
        protected void OnSettingsChanged(SettingsChangedEventArgs e)
        {
            if (this.NotifySettingsChanged != null)
            {
                this.NotifySettingsChanged(this, e);
            }
        }

        #endregion
    }
}
