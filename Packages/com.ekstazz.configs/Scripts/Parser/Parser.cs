namespace Ekstazz.Configs
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    
    internal class Parser : IConfigParser
    {
        public T ParseJson<T>(string text)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                NullValueHandling = NullValueHandling.Ignore
            };
            
            settings.Converters.Add(new StringEnumConverter());

            return JsonConvert.DeserializeObject<T>(text, settings);
        }
    }
}