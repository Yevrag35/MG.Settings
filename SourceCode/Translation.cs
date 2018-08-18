using MG;
using MG.Attributes;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace MG
{
    public sealed partial class AppSettings : MGNameResolver
    {
        internal RegistryValueKind Translate(object o)
        {
            Type t = o.GetType();
            Array allRegTypes = typeof(RegType).GetEnumValues();
            foreach (RegType reg in allRegTypes)
            {
                var rType = GetAttributeValues<DualAttribute>(reg);
                for (int e = 0; e < rType.Length; e++)
                {
                    object r = rType[e];
                    if (r.Equals(t))
                    {
                        var regKind = (RegistryValueKind)FromAttToNonMatch<MGNameAttribute>
                            (reg, typeof(RegistryValueKind))[0];
                        return regKind;
                    }
                }
            }
            throw new ArgumentException("No RegistryValueKind matches the type of " + t.FullName + "!");
        }
    }
}
