namespace Ekstazz.Saves.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class SaveModelConverter : JsonConverter
    {
        private const string HeaderKey = "header";

        private static readonly IDictionary<string, Type> SaveTypes = new Dictionary<string, Type>();

        static SaveModelConverter() 
        {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var types = assemblies
                    .SelectMany(ass => ass.GetTypes())
                    .Where(p => p.IsDefined(typeof(SaveComponentAttribute)));

                foreach (var type in types)
                {
                    var attribute = (SaveComponentAttribute) type.GetCustomAttribute(typeof(SaveComponentAttribute));
                    SaveTypes[attribute.Name] = type;
                }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var save = (SaveModel) value;

            writer.WriteStartObject();

            writer.WritePropertyName(HeaderKey);
            serializer.Serialize(writer, save.Header);

            foreach (var saveComponent in save.Components)
            {
                var pairs = SaveTypes.Where(valuePair => valuePair.Value == saveComponent.GetType());
                if (pairs.Any())
                {
                    var pair = pairs.First();
                    writer.WritePropertyName(pair.Key);
                    serializer.Serialize(writer, saveComponent);
                }
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var components = new Dictionary<Type, ISaveComponent>();
            SerializationHeader header = null;

            foreach (var entry in jObject)
            {
                var key = entry.Key;
                if (key == HeaderKey)
                {
                    if (header != null)
                    {
                        throw new Exception("Duplicate header in save file");
                    }
                    header = entry.Value.ToObject<SerializationHeader>(serializer);
                    continue;
                }

                if (!SaveTypes.ContainsKey(key))
                {
                    continue;
                }
                var saveType = SaveTypes[key];
                var component = entry.Value.ToObject(saveType, serializer);
                var saveComponent = component as ISaveComponent;
                if (saveComponent == null)
                {
                    throw new Exception($"Type {saveType} is not {nameof(ISaveComponent)}!");
                }

                components[saveType] = saveComponent;
            }

            if (header == null)
            {
                // Crashlytics.Log("Fixing missed header");
                header = new SerializationHeader(DateTime.UtcNow);
            }

            return new SaveModel(header, components.Values.ToList());
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SaveModel);
        }
    }
}