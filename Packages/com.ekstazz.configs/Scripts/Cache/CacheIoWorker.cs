namespace Ekstazz.Configs.Cache
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class CacheIoWorker
    {
        private readonly string filename = Path.Combine(Application.persistentDataPath, "cache.dat");

        public Task<byte[]> Read()
        {
            if (!File.Exists(filename))
            {
                return Task.FromResult<byte[]>(null);
            }
            return Task.FromResult(File.ReadAllBytes(filename));
        }
        
        public Task<bool> Write(byte[] data)
        {
            try
            {
                SafeWriteFile(data, filename);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return Task.FromResult(false);
            }
        }

        private static void SafeWriteFile(byte[] bytes, string filename)
        {
            var sourceFileName = $"{filename}.tmp";
            var backupFileName = $"{filename}.bak";
            File.WriteAllBytes(sourceFileName, bytes);
            SafeCommitFile(sourceFileName, filename, backupFileName);
        }

        private static void SafeCommitFile(string sourceFileName, string destinationFileName, string backupFileName)
        {
            File.Delete(backupFileName);
            if (File.Exists(destinationFileName))
            {
                File.Move(destinationFileName, backupFileName);
            }
            
            try
            {
                File.Move(sourceFileName, destinationFileName);
            }
            catch (Exception)
            {
                if (File.Exists(backupFileName))
                {
                    File.Move(backupFileName, destinationFileName);
                }
                throw;
            }
        }
    }
}