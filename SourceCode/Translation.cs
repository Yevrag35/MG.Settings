using MG;
using MG.Attributes;
using MG.Attributes.Exceptions;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace MG
{
    public partial class AppSettings : AttributeResolver
    {
        public RegistryValueKind Translate(object o)
        {
            Type t = o.GetType();
            if (t.IsEnum)
            {
                t = typeof(Enum);
            }
            var allRegTypes = GetEnumValues<RegType>();
            for (int r = 0; r < allRegTypes.Length; r++)
            {
                var rk = allRegTypes[r];

                var rTypes = GetAttributeValues<Type>(rk, typeof(TypeAttribute));
                for (int i = 0; i < rTypes.Length; i++)
                {
                    var rType = rTypes[i];
                    if (rType.Equals(t))
                    {
                        var regKind = GetAttributeValue<RegistryValueKind>(rk, typeof(IdentifierAttribute));
                        return regKind;
                    }
                }
            }
            throw new ArgumentException("No RegistryValueKind matches the type of " + t.FullName + "!");
        }
    }
}
