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
    /// <summary>
    /// A class for maintaining, editing, and saving configuration settings stored in JSON text files for use with WPF applications.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class JsonSettingsManager : IJsonSettings
    {
        #region PRIVATE FIELDS/CONSTANTS

        private const string JSON_EXT = ".json";

        private Encoding _enc;
        private string _filePath;
        [Obsolete]
        protected JObject InnerJob;
        //private JObject _job;
        //private JsonSerializerSettings _js;
        private JsonSerializer _forLoading;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// The text encoding used when reading from and writing to the JSON settings file.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public Encoding Encoding
        {
            get => _enc;
            set => _enc = value ?? throw new ArgumentNullException("Encoding");
        }
        /// <summary>
        /// Indicates whether the underlying JSON settings file exists.
        /// </summary>
        public bool Exists => !string.IsNullOrEmpty(_filePath);
        /// <summary>
        /// Specifies the full FileSystem path to the JSON settings file that will be read from and written to.
        /// </summary>
        /// <exception cref="ArgumentException">The path specified did have a .json file extension.</exception>
        /// <exception cref="ArgumentNullException"/>
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
        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
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
        /// <summary>
        /// An event handler for when changes that would alter the JSON settings have been made.
        /// </summary>
        public event SettingsChangedEventHandler NotifySettingsChanged;

        #endregion
        /// <summary>
        /// The default constructor using a UTF-8 <see cref="System.Text.Encoding"/>.
        /// </summary>
        public JsonSettingsManager()
        {
            _enc = Encoding.UTF8;
            //this.InnerJob = new JObject();
        }
        /// <summary>
        /// Initializes a <see cref="JsonSettingsManager"/> instance using a UTF-8 encoding with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path that the <see cref="JsonSettingsManager"/> will read from and write to.</param>
        public JsonSettingsManager(string filePath) : this() => this.FilePath = filePath;

        #region PUBLIC METHODS
        [Obsolete]
        public static T ReadFromFileAs<T>(string filePath, Encoding encoding, JsonSerializerSettings serializer)
            where T : IJsonSettings
        {
            using (var reader = new StreamReader(File.OpenRead(filePath), encoding))
            {
                string rawJson = reader.ReadToEnd();
                return serializer != null
                ? JsonConvert.DeserializeObject<T>(rawJson, serializer)
                : JsonConvert.DeserializeObject<T>(rawJson);
            }
        }
        void IReadable.Read() => this.Read();
        /// <summary>
        /// Reads the file from <see cref="JsonSettingsManager.FilePath"/> and dynamically maps the properties to inheriting class.
        /// </summary>
        /// <param name="action">The specific action to be presented to the event handler.</param>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException">The <see cref="JsonSettingsManager.FilePath"/> property has not been defined.</exception>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="IOException" />
        /// <exception cref="NotSupportedException" />
        /// <exception cref="OutOfMemoryException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="UnauthorizedAccessException" />
        public virtual void Read(SettingChangedAction action = SettingChangedAction.Read)
        {
            if (string.IsNullOrEmpty(_filePath))
                throw new ArgumentNullException("FilePath");

            using (var reader = new StreamReader(File.OpenRead(_filePath), _enc))
            {
                string rawJson = reader.ReadToEnd();

                if (this.JsonSerializer != null)
                    JsonConvert.PopulateObject(rawJson, this, this.JsonSerializer);

                else
                    JsonConvert.PopulateObject(rawJson, this);

                this.OnSettingsChanged(new SettingsChangedEventArgs(action));
            }
        }
        /// <summary>
        /// Serializes the <see cref="JsonSettingsManager"/> instance into JSON and writes the text to the path 
        /// defined in <see cref="JsonSettingsManager.FilePath"/>.
        /// </summary>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// <exception cref="NotSupportedException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="UnauthorizedAccessException" />
        public virtual void Save()
        {
            using (var streamWriter = new StreamWriter(File.OpenWrite(_filePath), _enc))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter)
                {
                    AutoCompleteOnClose = true,
                    CloseOutput = true,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    Formatting = Formatting.Indented,
                    Indentation = 1
                })
                {
                    if (char.TryParse("\t", out char indentChar))
                    {
                        jsonWriter.IndentChar = indentChar;
                    }
                    ((IJsonSettings)this).Save(jsonWriter);
                }
            }
        }
        void ISavable.Save(JsonTextWriter jsonWriter)
        {
            var converter = new StringEnumConverter(new CamelCaseNamingStrategy());
            ((IJsonSettings)this).SettingsAsJson.WriteTo(jsonWriter, converter);
        }
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
