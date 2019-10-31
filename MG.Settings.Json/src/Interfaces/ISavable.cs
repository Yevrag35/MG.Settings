using Newtonsoft.Json;
using System;

namespace MG.Settings.Json
{
    public interface ISavable
    {
        void Save();
        void Save(JsonTextWriter jsonWriter);
    }
}
