using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Settings.JsonSettings
{
    public interface IJsonRemover
    {
        event JsonConfigEventHandler ConfigRemoved;

        void RemoveSetting(string settingName);
    }
}
