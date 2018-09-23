using Microsoft.Win32;
using System;

namespace MG.Attributes
{
    public class IdentifierAttribute : MGAbstractAttribute
    {
        public IdentifierAttribute(RegistryValueKind[] regKind)
            : base(regKind)
        {
        }
    }

    public class TypeAttribute : MGAbstractAttribute
    {
        public TypeAttribute(Type[] type)
            : base(type)
        {
        }
    }
}
