using System;
using System.Collections.Generic;

namespace MG.Settings.JsonSettings
{
    public delegate void PropertyMapperEventHandler(object sender, PropertyMapperEventArgs e);

    public class PropertyMapperEventArgs : EventArgs
    {
        public IJsonMapper CurrentMapper { get; }
        public IJsonMapper PreviousMapper { get; }
        public PropertyMappingType MappingType { get; }
        public object Setting { get; }

        public PropertyMapperEventArgs(PropertyMappingType mappingType) => MappingType = mappingType;
        public PropertyMapperEventArgs(PropertyMappingType mappingType, IJsonMapper currentMapper, IJsonMapper previous)
            : this(mappingType)
        {
            CurrentMapper = currentMapper;
            PreviousMapper = previous;
        }
        public PropertyMapperEventArgs(PropertyMappingType mappingType, object setting)
            : this(mappingType) => Setting = setting;
    }

    public enum PropertyMappingType
    {
        MatchToInteralSet = 0,
        MatchToPrivateFields = 1
    }
}
