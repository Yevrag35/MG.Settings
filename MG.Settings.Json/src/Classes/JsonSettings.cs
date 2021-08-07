using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MG.Settings.Json.Extensions;

namespace MG.Settings.Json
{
    /// <summary>
    /// A class for reading and saving settings for applications in a JSON file format.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class JsonSettings : IJsonSettings, IAsyncReadable, IAsyncSavable, IJsonConvertible
    {
        #region FIELDS/CONSTANTS
        private DirectoryInfo _defaultFolder;
        private FileInfo _defaultFileInfo;
        private Encoding _encoding;
        private IIndentSettings _formatting;
        private DirectoryInfo _folderInfo;
        private FileInfo _fileInfo;
        private JsonSerializer _serializer;
        private JsonSerializerSettings _serializerSettings;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Specifies the default file path for the settings file if the configured <see cref="FilePath"/> is not
        /// found or is impossible to write to.  If this path is not configured and <see cref="FilePath"/> is invalid,
        /// then an error will be thrown.
        /// </summary>
        public string DefaultFilePath
        {
            get => _defaultFileInfo.FullName;
            set
            {
                _defaultFileInfo = new FileInfo(value);
                _defaultFolder = new DirectoryInfo(_defaultFileInfo.DirectoryName);
            }
        }

        public object DefaultSettings { get; set; }

        /// <summary>
        /// The encoding used when reading and writing to the JSON settings file.
        /// Defaults to <see cref="Encoding.UTF8"/> and cannot be made <see langword="null"/>.
        /// </summary>
        public Encoding Encoding
        {
            get => _encoding;
            set
            {
                if (value != null)
                    _encoding = value;
            }
        }

        /// <summary>
        /// Specifies the full FileSystem path to the JSON settings file that will be read from and written to.
        /// </summary>
        public string FilePath
        {
            get => _fileInfo.FullName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _fileInfo = new FileInfo(value);
                    _folderInfo = new DirectoryInfo(_fileInfo.DirectoryName);
                }
            }
        }

        public IIndentSettings JsonFormatting
        {
            get
            {
                if (_formatting == null)
                    _formatting = JsonFormat.Default;

                return _formatting;
            }
            set => _formatting = value;
        }

        /// <summary>
        /// The <see cref="JsonSerializer"/> to be used when converting between <see cref="JToken"/> instances.
        /// </summary>
        public JsonSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                    _serializer = new JsonSerializer();

                return _serializer;
            }
            set => _serializer = value;
        }

        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        public JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (_serializerSettings == null)
                    _serializerSettings = new JsonSerializerSettings();

                return _serializerSettings;
            }
            set => _serializerSettings = value;
        }

        #endregion

        #region CONSTRUCTORS
        public JsonSettings(string folderPath, string fileName)
            : this(folderPath, fileName, Encoding.UTF8, null, false)
        {
        }
        public JsonSettings(string folderPath, string fileName, Encoding encoding, object defaultSettings, bool forceCreate,
            JsonSerializer serializer = null)
        {
            this.Encoding = encoding;
            this.DefaultSettings = defaultSettings;
            this.DefaultFilePath = Path.Combine(folderPath, fileName);
            this.Serializer = serializer;

            if (!this.TestDefaultPath())
            {
                if (forceCreate)
                {
                    _defaultFolder = Directory.CreateDirectory(_defaultFolder.FullName);

                    if (defaultSettings != null)
                    {
                        this.InitialSave(defaultSettings);
                        _defaultFileInfo.Refresh();
                    }
                }
            }

            _folderInfo = _defaultFolder;
            _fileInfo = _defaultFileInfo;
        }


        #endregion

        #region EVENT HANDLERS - FUTURE

        // FUTURE TO DO
        
        ///// <summary>
        ///// An event handler for when changes that would alter the JSON settings have been made.
        ///// </summary>
        //public event SettingsChangedEventHandler NotifySettingsChanged;

        #endregion

        #region PUBLIC METHODS
        public bool CanRead(out Exception caughtException)
        {
            caughtException = null;
            bool result = false;

            if (_fileInfo == null || File.Exists(_fileInfo.FullName))
                return result;

            try
            {
                using (FileStream fs = File.OpenRead(_fileInfo.FullName))
                {
                    result = fs.CanRead;
                }
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            return result;
        }

        public bool CanSave(out Exception caughtException)
        {
            caughtException = null;
            bool result = false;

            if (_fileInfo == null || !File.Exists(_fileInfo.FullName))
                return result;

            try
            {
                using (FileStream fs = File.Open(_fileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    result = fs.CanWrite;
                }
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            return result;
        }

        public bool DefaultExists()
        {
            _defaultFolder.Refresh();
            _defaultFileInfo.Refresh();
            return _defaultFolder.Exists && _defaultFileInfo.Exists;
        }

        /// <summary>
        /// Indicates whether the configured <see cref="FilePath"/> exists.
        /// </summary>
        /// <remarks>
        ///     If an error occurs during the check,
        ///     the caught <see cref="IOException"/> is returned as <paramref name="caughtException"/>.
        /// </remarks>
        /// <returns>
        ///     <see langword="true"/> if the path at <see cref="FilePath"/> exists;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public bool Exists(out IOException caughtException)
        {
            caughtException = null;
            if (_fileInfo == null)
                return false;

            bool result = false;

            try
            {
                _fileInfo.Refresh();
                result = _fileInfo.Exists;
            }
            catch (IOException io)
            {
                caughtException = io;
            }

            return result;
        }

        /// <summary>
        /// Reads the configured JSON file and deserializes it into the current object.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="PathTooLongException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="IOException"/>
        public void Read()
        {
            string jsonContents = this.ReadFile(_fileInfo.FullName);
            if (string.IsNullOrWhiteSpace(jsonContents))
                return;

            JsonConvert.PopulateObject(jsonContents, this, this.SerializerSettings);
        }

        /// <summary>
        /// Reads the configured JSON file and deserializes it a new <see cref="JObject"/>.
        /// </summary>
        public JObject ToJObject(JsonLoadSettings loadSettings)
        {
            if (loadSettings == null)
                loadSettings = new JsonLoadSettings
                {
                    DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error
                };

            string jsonContent = this.ReadFile(_fileInfo.FullName);
            
            return !string.IsNullOrWhiteSpace(jsonContent)
                ? JObject.Parse(jsonContent, loadSettings)
                : null;
        }
        public JObject ToJObject() => this.ToJObject(new JsonLoadSettings
        {
            DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error
        });

        /// <summary>
        /// Reads the configured JSON file asynchronously and deserializes it into the current object.
        /// </summary>
        public async Task ReadAsync()
        {
            string jsonContents = await this.ReadFileAsync(_fileInfo.FullName);
            if (string.IsNullOrWhiteSpace(jsonContents))
                return;

            JsonConvert.PopulateObject(jsonContents, this, this.SerializerSettings);
        }

        public void InitialSave(object objToSerialize)
        {
            this.WriteFile(this.DefaultFilePath, objToSerialize);
        }

        /// <summary>
        /// Serializes the data held by the inheriting class into JSON and writes it to the configured file.
        /// </summary>
        public void Save() => this.WriteFile(_fileInfo.FullName, this);
        /// <summary>
        /// Serializes the data held by the inheriting class into JSON and writes it to the configured file asynchronously.
        /// </summary>
        public async Task SaveAsync() => await this.WriteFileAsync(_fileInfo.FullName, this);

        #endregion

        #region PROTECTED METHODS


        #endregion

        #region BACKEND/PRIVATE METHODS

        private string ReadFile(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(fs, this.Encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        private async Task<string> ReadFileAsync(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(fs, this.Encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        //private bool TestFilePath(string path, out FileInfo foundFileInfo)
        //{
        //    foundFileInfo = null;
        //    if (string.IsNullOrWhiteSpace(path))
        //        return false;

        //    bool exists = File.Exists(path);
        //    foundFileInfo = new FileInfo(path);

        //    return exists;
        //}

        private bool TestDefaultPath() => File.Exists(this.DefaultFilePath);

        private void WriteFile(string filePath, object objToWrite)
        {
            using (FileStream fs = File.Create(filePath))
            {
                using (var streamWriter = new StreamWriter(fs, this.Encoding))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter)
                    {
                        Formatting = this.JsonFormatting.Formatting,
                        Indentation = this.JsonFormatting.IndentCount,
                        IndentChar = this.JsonFormatting.IndentChar
                    })
                    {
                        JObject job = JObject.FromObject(objToWrite, this.Serializer);
                        job.WriteTo(jsonTextWriter);
                    }
                }
            }
        }
        private async Task WriteFileAsync(string filePath, IJsonConvertible objToWrite)
        {
            using (FileStream fs = File.Create(filePath))
            {
                using (var streamWriter = new StreamWriter(fs, this.Encoding))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter)
                    {
                        Formatting = this.JsonFormatting.Formatting,
                        Indentation = this.JsonFormatting.IndentCount,
                        IndentChar = this.JsonFormatting.IndentChar
                    })
                    {
                        JObject job = JObject.FromObject(objToWrite, this.Serializer);
                        await job.WriteToAsync(jsonTextWriter);
                    }
                }
            }
        }
        

        #endregion
    }
}