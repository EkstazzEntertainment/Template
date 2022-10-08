namespace Ekstazz.Saves.Packers
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// This class knows about all bson-base64-crypto stuff
    /// It must handle both encoded and raw data (!) and return readable json
    /// </summary>
    internal interface ISavePacker
    {
        JObject Unpack(byte[] raw);

        byte[] Pack(JObject json);
    }
}