﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Settings.JsonSettings
{
    public interface IJsonWriter
    {
        event JsonConfigEventHandler ConfigWrittenTo;

        bool WriteSetting(string settingName, object settingValue);
    }
}