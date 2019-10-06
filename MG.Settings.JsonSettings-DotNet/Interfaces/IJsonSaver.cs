using System;

namespace MG.Settings.JsonSettings
{
    public interface IJsonSaver
    {
        event JsonConfigEventHandler ConfigSaved;
        void SaveConfig();
    }
}
