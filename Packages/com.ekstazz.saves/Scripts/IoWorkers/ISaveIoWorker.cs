namespace Ekstazz.Saves
{
    using System.Threading.Tasks;

    /// <summary>
    /// This interface implementations should not know anything about
    /// cyphers, data models, jsons, serialization etc. - only read\write, input\output
    /// </summary>
    public interface ISaveIoWorker : ISaveIoReader, ISaveIoWriter
    {
    }

    public interface ISaveIoReader
    {
        Task<byte[]> Read(string key = null);
    }

    public interface ISaveIoWriter
    {
        Task<bool> Write(byte[] data, string key = null);
    }
}