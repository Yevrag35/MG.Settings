using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface that provides a method to read the underlying JSON file into the specified <see cref="IJsonConvertible"/>.
    /// </summary>
    public interface IReadable<T> where T : IJsonConvertible
    {

    }
}
