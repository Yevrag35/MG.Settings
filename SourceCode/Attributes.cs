using MG.Attributes.Interfaces;
using Microsoft.Win32;
using System;

namespace MG
{
    //public class IdentifierAttribute : Attribute, IAttribute
    //{
    //    private RegistryValueKind _regKind;
    //    public IdentifierAttribute(RegistryValueKind regKind)
    //    {
    //        _regKind = regKind;
    //    }
    //    public object Value => _regKind;
    //}
    public class TypeAttribute : Attribute, IAttribute
    {
        private Type _t;
        public TypeAttribute(Type type)
        {
            _t = type;
        }
        public object Value => _t;
    }
}
