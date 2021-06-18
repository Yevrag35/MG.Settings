using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface exposing methods and properties for a JSON setting manipulator.
    /// </summary>
    public interface IJsonSettings : IReadable, ISavable
    {
        /// <summary>
        /// An event handler for when changes that would alter the JSON settings have been made.
        /// </summary>
        event SettingsChangedEventHandler NotifySettingsChanged;

        /// <summary>
        /// The text encoding used when reading from and writing to the JSON settings file.
        /// </summary>
        Encoding Encoding { get; set; }
        /// <summary>
        /// Specifies the full FileSystem path to the JSON settings file that will be read from and written to.
        /// </summary>
        string FilePath { get; set; }
        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        JsonSerializerSettings SerializerSettings { get; set; }
        /// <summary>
        /// Sets the <see cref="Newtonsoft.Json.JsonSerializer"/> to be used when converting between <see cref="JToken"/> instances.
        /// </summary>
        JsonSerializer Serializer { get; set; }
        /// <summary>
        /// Returns the represented settings from the inheriting class as a <see cref="JObject"/>.
        /// </summary>
        //JObject SettingsAsJson { get; }
        JObject GetAsJObject();
    }
}
