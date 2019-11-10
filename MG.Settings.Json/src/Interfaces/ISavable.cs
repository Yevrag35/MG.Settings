using Newtonsoft.Json;
using System;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface providing methods for serializing data back to the file system.
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// Serializes the data held by the inheriting class into JSON and writes it to the configured file.
        /// </summary>
        void Save();

        /// <summary>
        /// Serializes the data using the specified <see cref="JsonTextWriter"/> held by the 
        /// inheriting class into JSON and writes it to the configured file.
        /// </summary>
        /// <param name="jsonWriter">The text writer to use when serializing the data.</param>
        void Save(JsonTextWriter jsonWriter);
    }
}
