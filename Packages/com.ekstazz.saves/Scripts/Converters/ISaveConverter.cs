namespace Ekstazz.Saves.Converters
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// This class must take save json from any version and return
    /// readable json of latest version
    /// </summary>
    public interface ISaveConverter
    {
        void AddConverter(IncrementalSaveConverter converter);
        
        JObject ConvertToLatest(JObject json);
    }
}