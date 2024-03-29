﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MG.Settings.Json
{
    /// <summary>
    /// An interface that provides a method to read the underlying JSON file.
    /// </summary>
    public interface IReadable
    {
        /// <summary>
        /// Specifies custom <see cref="JsonSerializerSettings"/> used when reading and writing the JSON settings file.
        /// </summary>
        JsonSerializerSettings SerializerSettings { get; set; }

        bool CanRead(out Exception caughtException);

        /// <summary>
        /// Reads the configured JSON file and deserializes it into the current inheriting instance.
        /// </summary>
        void Read();
    }
}
