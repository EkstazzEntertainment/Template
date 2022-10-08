namespace Ekstazz.Saves
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Serialization
    {
        public static readonly int CurrentVersion;
        
        public static readonly int MinimumVersion;
        
        public static readonly List<SerializationTypeMapping> saveGameTypeMapping = new List<SerializationTypeMapping>();
        
        static Serialization()
        {
            var version = Resources.Load<SerializationVersion>($"Settings/{nameof(SerializationVersion)}");
            CurrentVersion = version.currentVersion;
            MinimumVersion = version.minimumVersion;
        }
        
        public struct SerializationTypeMapping
        {
            public string assemblyName;
            public string typeName;
            public Type type;

            public SerializationTypeMapping (string assemblyName, string typeName, Type type)
            {
                this.assemblyName = assemblyName;
                this.typeName = typeName;
                this.type = type;
            }
        }
    }
}