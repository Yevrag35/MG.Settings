using MG.Exceptions;
using System;
using System.Collections;

namespace MG
{
    public sealed partial class AppSettings
    {
        private const string kv = "RegKey";
        public AppSettings(IDictionary propsAndDefVals)
        {
            if (!propsAndDefVals.Contains(kv))
            {
                throw new NoMGRegPathInObjectException("The specified key/value pair 'RegKey' was not found in the supplied IDictionary!");
            }
            else
            {
                rkstr = (string)propsAndDefVals[kv];
            }
            propsAndDefVals.Remove(kv);
            _mg = _cu.CreateSubKey(mgstr);
            _cu.Close();
            _cu.Dispose();
            _rk = _mg.CreateSubKey(rkstr);
            _mg.Close();
            _mg.Dispose();
            SetProperties(propsAndDefVals);
        }

        public AppSettings(string regKeyRoot, IDictionary propsAndDefVals)
        {
            if (string.IsNullOrEmpty(rkstr))
            {
                rkstr = regKeyRoot;
            }
            _mg = _cu.CreateSubKey(mgstr);
            _cu.Close();
            _cu.Dispose();
            _rk = _mg.CreateSubKey(rkstr);
            _mg.Close();
            _mg.Dispose();
            SetProperties(propsAndDefVals);
        }
    }
}
