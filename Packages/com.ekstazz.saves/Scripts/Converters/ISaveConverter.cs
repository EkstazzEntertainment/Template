namespace Ekstazz.Saves.Converters
{
    using Newtonsoft.Json.Linq;

    
    public interface ISaveConverter
    {
        void AddConverter(IncrementalSaveConverter converter);
        JObject ConvertToLatest(JObject json);
    }
}