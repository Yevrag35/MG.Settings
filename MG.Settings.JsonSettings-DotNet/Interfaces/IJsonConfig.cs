using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public interface IJsonConfig
    {
        event JsonConfigEventHandler JsonConfigChanged;
    }
}
