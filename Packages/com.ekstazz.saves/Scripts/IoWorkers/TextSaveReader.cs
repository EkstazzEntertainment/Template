namespace Ekstazz.Saves
{
    using System;
    using System.Threading.Tasks;

    
    public class StringSaveReader : ISaveIoReader
    {
        public Task<byte[]> Read(string key = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            
            return Task.FromResult(Convert.FromBase64String(key));
        }
    }
}