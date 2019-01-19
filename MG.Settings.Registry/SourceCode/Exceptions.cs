using System;

namespace MG.Exceptions
{
    public sealed class NoMGRegPathInObjectException : Exception
    {
        public NoMGRegPathInObjectException(string message)
            : base(message)
        {
        }
    }
}
