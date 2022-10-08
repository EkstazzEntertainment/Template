namespace Ekstazz.Configs
{
    using UnityEngine.Scripting;

    internal interface IConfigParser
    {
//        T[] ParseStringToObjects<T> (string text) where T : new();

        //Dictionary<TKey, T> ParseStringToDictionary<TKey, T> (string text) where T : new();

        [Preserve]
        T ParseJson<T>(string text);
//
//        string ParseFromString(string input);
//
//        Dictionary<TKey, T> ParseStringToDictionaryWithCompositeKey<TKey, T> (string text)
//            where T : new()
//            where TKey : new();
    }
}