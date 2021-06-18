using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MG.Settings.Json
{
    /// <summary>
    /// A class for reading, storing, and saving settings for applications in a JSON file format.
    /// </summary>
    public class JsonSettings : IJsonSettings
    {
        #region FIELDS/CONSTANTS
        private FileInfo _fileInfo;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// The encoding used when reading and writing to the JSON settings file.
        /// Defaults to <see cref="Encoding.UTF8"/>.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Specifies the full FileSystem path to the JSON settings file that will be read from and written to.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The <see cref="JsonSerializer"/> to be used when converting between <see cref="JToken"/> instances.
        /// </summary>
        public JsonSerializer Serializer { get; set; }

        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; }

        #endregion

        #region CONSTRUCTORS
        public JsonSettings(string folderPath, string fileName)
        {
        }
        public JsonSettings(string folderPath, string fileName, Encoding encoding)
        {
            this.Encoding = encoding;
            
        }
        



        #endregion

        #region EVENT HANDLERS
        public event SettingsChangedEventHandler NotifySettingsChanged;

        #endregion

        #region PUBLIC METHODS
        
        public JObject GetAsJObject() => throw new NotImplementedException();
        public void Read() => throw new NotImplementedException();
        public void Save() => throw new NotImplementedException();
        public void Save(JsonTextWriter jsonWriter) => throw new NotImplementedException();
        public void SetLoadSerializer(JsonSerializer loadSerializer) => throw new NotImplementedException();

        #endregion

        #region BACKEND/PRIVATE METHODS


        #endregion
    }
}