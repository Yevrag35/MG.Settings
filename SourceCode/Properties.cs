using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MG
{


    public partial class AppSettings
    {
        private const string mgstr = "Mike Garvey";
        private string rkstr;
        private RegistryKey _cu = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
        private RegistryKey _mg;
        private RegistryKey _rk;
        public OrderedDictionary Properties
        {
            get { return _vals; }
        }
        private OrderedDictionary _vals { get; set; }
    }
}
