namespace Ekstazz.Saves.Packers
{
    using Newtonsoft.Json.Linq;


    internal interface ISavePacker
    {
        JObject Unpack(byte[] raw);
        byte[] Pack(JObject json);
    }
}