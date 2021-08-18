using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface providing methods for serializing data back to the file system asynchronously.
    /// </summary>
    public interface IAsyncSavable
    {
        /// <summary>
        /// Sets the <see cref="Newtonsoft.Json.JsonSerializer"/> to be used when converting between <see cref="JToken"/> instances.
        /// </summary>
        JsonSerializer Serializer { get; set; }

        /// <summary>
        /// Serializes the data held by the inheriting class into JSON and writes it to the configured file asynchronously.
        /// </summary>
        Task SaveAsync();
    }
}
