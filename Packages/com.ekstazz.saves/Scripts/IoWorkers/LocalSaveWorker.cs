namespace Ekstazz.Saves
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;

    
    internal class LocalSaveWorker : ISaveIoWorker
    {
        private readonly string filename = Path.Combine(Application.persistentDataPath, "save.dat");

        
        public Task<byte[]> Read(string key)
        {
            if (!File.Exists(filename))
            {
                return Task.FromResult<byte[]>(null);
            }

            return Task.FromResult(File.ReadAllBytes(filename));
        }

        public Task<bool> Write(byte[] data, string key)
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

        private static void SafeCommitFile(string sourceFileName, string destinationFileName, string backupFileName = null)
        {
            if (string.IsNullOrEmpty(sourceFileName))
            {
                throw new ArgumentException("sourceFileName is null");
            }

            if (string.IsNullOrEmpty(destinationFileName))
            {
                throw new ArgumentException("destinationFileName is null");
            }

            if (backupFileName == null)
            {
                backupFileName = destinationFileName + ".bak";
            }

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
                    File.Move(backupFileName, destinationFileName); // Try to move old file back.
                }

                throw;
            }
        }
    }
}