using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MG.Settings.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ConvertibleJsonBase : IJsonConvertible
    {
        public virtual JObject ToJObject(JsonSerializer serializer)
        {
            return JObject.FromObject(this, serializer);
        }
        public abstract JObject ToJObject();
    }
}