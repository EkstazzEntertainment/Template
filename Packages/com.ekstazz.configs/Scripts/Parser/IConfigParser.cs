namespace Ekstazz.Configs
{
    using UnityEngine.Scripting;

    internal interface IConfigParser
    {
        [Preserve]
        T ParseJson<T>(string text);
    }
}