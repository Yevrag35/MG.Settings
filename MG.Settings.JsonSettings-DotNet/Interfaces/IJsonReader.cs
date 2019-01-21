using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.JsonSettings
{
    public interface IJsonReader
    {

        IJsonConfig ReadConfig();
    }
}
