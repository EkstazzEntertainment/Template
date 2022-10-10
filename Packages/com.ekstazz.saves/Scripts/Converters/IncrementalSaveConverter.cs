namespace Ekstazz.Saves.Converters
{
    using Newtonsoft.Json.Linq;

    
    public abstract class IncrementalSaveConverter
    {
        public bool CanConvert(JObject jObject)
        {
            var header = jObject["header"];
            if (header == null)
            {
                return false;
            }
            var version = header["serializationVersion"].Value<int>();
            return CanConvertFromVersion(version);
        }

        protected abstract bool CanConvertFromVersion(int version);
        public abstract int GoalVersion { get; }

        public void Convert(JObject jObject)
        {
            jObject["header"]["serializationVersion"] = $"{GoalVersion}";
            ConvertInternal(jObject);
        }

        protected abstract void ConvertInternal(JObject jObject);
    }
}