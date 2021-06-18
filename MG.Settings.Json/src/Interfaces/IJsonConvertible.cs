using Newtonsoft.Json.Linq;

namespace MG.Settings.Json
{
    public interface IJsonConvertible
    {
        JObject ToJObject();
    }
}
