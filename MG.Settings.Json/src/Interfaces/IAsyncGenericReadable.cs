using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface that provides a method to read the underlying JSON file asynchronously into the specified <see cref="IJsonConvertible"/>.
    /// </summary>
    public interface IAsyncGenericReadable
    {
        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        JsonSerializerSettings SerializerSettings { get; set; }

        Task<T> ReadAsync<T>(string filePath) where T : IJsonConvertible;
    }
}
