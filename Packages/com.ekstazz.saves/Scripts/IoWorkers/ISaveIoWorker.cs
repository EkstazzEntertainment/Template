namespace Ekstazz.Saves
{
    using System.Threading.Tasks;


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