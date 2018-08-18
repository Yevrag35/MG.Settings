using Microsoft.Win32;
using System;

namespace MG.Attributes
{
    public abstract class DualAttribute : MGAbstractAttribute
    {
        public DualAttribute(object o)
            : base(o)
        {
        }
    }

    public class IdentifierAttribute : MGAbstractAttribute
    {
        public IdentifierAttribute(RegistryValueKind regKind)
            : base(regKind)
        {
        }
    }

    public class TypeAttribute : DualAttribute
    {
        public TypeAttribute(Type type)
            : base(type)
        {
        }
    }
    public class MultiTypeAttribute : DualAttribute
    {
        public MultiTypeAttribute(Type[] types)
            : base(types)
        {
        }
    }
}
