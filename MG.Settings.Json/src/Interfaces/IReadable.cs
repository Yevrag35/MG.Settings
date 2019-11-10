using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface that provides a method to read the underlying JSON file.
    /// </summary>
    public interface IReadable
    {
        /// <summary>
        /// Reads the configured JSON file and deserializes it into the current object.
        /// </summary>
        void Read();
    }
}
