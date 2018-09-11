using MG;
using MG.Attributes;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace MG
{
    public partial class AppSettings : MGNameResolver
    {
        public RegistryValueKind Translate(object o)
        {
            Type t = o.GetType();
            if (t.IsEnum)
            {
                t = typeof(Enum);
            }
            Array allRegTypes = typeof(RegType).GetEnumValues();
            for (int i = 0; i < allRegTypes.Length; i++)
            {
                var workingEnum = (RegType)allRegTypes.GetValue(i);
                var rt = GetAttributeValues<TypeAttribute>(workingEnum);
                for (int e = 0; e < rt.Length; e++)
                {
                    var enumType = (Type)rt[e];
                    if (enumType.Equals(t))
                    {
                        var regKind = (RegistryValueKind)GetAttributeValues<IdentifierAttribute>(workingEnum)[0];
                        return regKind;
                    }
                }
            }
            throw new ArgumentException("No RegistryValueKind matches the type of " + t.FullName + "!");
        }
    }
}
