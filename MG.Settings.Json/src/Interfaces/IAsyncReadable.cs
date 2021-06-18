using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface that provides a method to read the underlying JSON file asynchronously.
    /// </summary>
    public interface IAsyncReadable
    {
        bool CanRead(out Exception caughtException);

        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        JsonSerializerSettings SerializerSettings { get; set; }

        /// <summary>
        /// Reads the configured JSON file asynchronously and deserializes it into the current object.
        /// </summary>
        Task ReadAsync();
    }
}
