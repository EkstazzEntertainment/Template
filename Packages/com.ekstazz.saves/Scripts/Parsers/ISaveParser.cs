namespace Ekstazz.Saves.Parsers
{
    using Data;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Converts valid json of latest version into DataModel
    /// </summary>
    internal interface ISaveParser
    {
        SaveModel FromJson(JObject json);

        JObject ToJson(SaveModel saveModel);
    }
}