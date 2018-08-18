using MG;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace MG
{
    public partial class AppSettings : MGNameResolver
    {
        //private Enum GetByType(Type type)
        //{
        //    foreach (RegType r in (new RegType[] { RegType.Binary, RegType.DWord, RegType.String, RegType.QWord, RegType.MultiString }))
        //    {
        //        FieldInfo fi = r.GetType().GetField(r.ToString());
        //        TypeAttribute ta = ((fi.GetCustomAttributes(typeof(TypeAttribute), false)) as TypeAttribute[])[0];
        //        if (ta.Value.Equals(type))
        //        {
        //            return r;
        //        }
        //    }
        //    // if you got here, something's wrong...
        //    return null;
        //}

        //private RegistryValueKind GetRegKindValue(Enum _enum)
        //{
        //    Type type = _enum.GetType();
        //    FieldInfo fi = type.GetField(_enum.ToString());
        //    IdentifierAttribute att = ((fi.GetCustomAttributes(typeof(IdentifierAttribute), false)) as IdentifierAttribute[])[0];
        //    return att.Value;
        //}

        //private RegistryValueKind Translate(object input)
        //{
        //    Type inType = input.GetType();
        //    object o = MatchEnums(inType, typeof(RegType), typeof(TypeAttribute));
        //    return (RegistryValueKind)GetValue(o, typeof(IdentifierAttribute));
        //}
    }
}
