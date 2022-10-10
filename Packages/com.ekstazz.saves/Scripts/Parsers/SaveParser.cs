namespace Ekstazz.Saves.Parsers
{
    using System.Runtime.Serialization;
    using Data;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    
    
    internal class SaveParser : ISaveParser
    {
        public SaveModel FromJson(JObject json)
        {
            if (json == null)
            {
                return null;
            }

            var model = json.ToObject<SaveModel>(CreateSerializer());
            return model;
        }

        public JObject ToJson(SaveModel saveModel)
        {
            return JObject.FromObject(saveModel, CreateSerializer());
        }

        private JsonSerializer CreateSerializer()
        {
            var serializer = JsonSerializer.Create
            (new JsonSerializerSettings
            {
                Context = new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence),
                NullValueHandling = NullValueHandling.Ignore
            });

            serializer.Converters.Add(new SaveModelConverter());
            serializer.Converters.Add(new StringEnumConverter());
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

            return serializer;
        }
    }
}