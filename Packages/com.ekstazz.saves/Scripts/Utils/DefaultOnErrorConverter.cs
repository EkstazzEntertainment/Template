namespace Ekstazz.Utils
{
    using System;
    using Newtonsoft.Json;

    public abstract class DefaultOnErrorConverter<T> : JsonConverter
    {
        protected abstract T ErrorValue { get; }

        protected abstract JsonConverter DefaultConverter { get; }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DefaultConverter.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return DefaultConverter.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch (Exception)
            {
                return ErrorValue;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}