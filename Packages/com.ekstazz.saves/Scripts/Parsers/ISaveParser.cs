namespace Ekstazz.Saves.Parsers
{
    using Data;
    using Newtonsoft.Json.Linq;


    internal interface ISaveParser
    {
        SaveModel FromJson(JObject json);
        JObject ToJson(SaveModel saveModel);
    }
}