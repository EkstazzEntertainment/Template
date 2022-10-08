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

            //do not store serialaizer as a field - it would fail on 
            //parsing next instances due to referenceresolver reusage
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
//                ContractResolver = SaveGameContractResolver.Instance,
                Context = new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence),
                NullValueHandling = NullValueHandling.Ignore
            });

//            serializer.Binder = new SaveGameSerializationBinder(serializer.Binder, Serialization.saveGameTypeMapping);
//            serializer.ReferenceResolver = new SimpleReferenceResolver();
            
            serializer.Converters.Add(new SaveModelConverter());
//            serializer.Converters.Add(new CurrencyTypeErrorConverter());
            
//            serializer.Converters.Add(new ShopItemTypeErrorConverter());
            serializer.Converters.Add(new StringEnumConverter());
            
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;

            return serializer;
        }
    }
}