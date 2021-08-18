using Newtonsoft.Json;
using System;

namespace MG.Settings.Json
{
    public interface IIndentSettings
    {
        Formatting Formatting { get; }
        char IndentChar { get; }
        int IndentCount { get; }
    }
}
