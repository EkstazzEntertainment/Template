namespace Ekstazz.Saves.Data
{
    using System;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    internal class SerializationHeader
    {
        [JsonProperty("saveDate")]
        public DateTime SaveTimestamp { get; private set; }

        [JsonProperty("serializationVersion"), Preserve]
        public int Version { get; private set; }

        public SerializationHeader(DateTime saveTimestamp)
        {
            SaveTimestamp = saveTimestamp;
            Version = Serialization.CurrentVersion;
        }
    }
}