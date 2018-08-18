using System;

namespace MG.Exceptions
{
    public class NoMGRegPathInObjectException : Exception
    {
        public NoMGRegPathInObjectException(string message)
            : base(message)
        {
        }
    }
}
