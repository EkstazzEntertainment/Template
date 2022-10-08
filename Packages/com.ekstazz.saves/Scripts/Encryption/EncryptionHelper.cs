namespace Ekstazz.Saves.Encryption
{
    using System.IO;
    using System.Security.Cryptography;

    // Encryption Helper.
    internal static class EncryptionHelper
    {
        // CE - Create Encryptor: Creates AES-128 encryptor, stream and writes IV.
        //
        // Note: We do not storing any size and do not truncating stream created by CD() function on reading
        // so it can contain some "garbage" created by padding.
        public static CryptoStream Encryptor (byte[] key, Stream stream, int keySize)
        {
            var aes = new AesManaged()
            {
                BlockSize = 128,
                KeySize = keySize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            aes.GenerateIV();
            aes.Key = key;

            stream.Write(aes.IV, 0, aes.IV.Length);
            stream.Flush();

            return new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        }

        // CD - Create Decryptor: Creates AES-128 decryptor, stream and reads IV.
        //
        // Note: We do not storing any size and do not truncating stream created by CD() function on reading
        // so it can contain some "garbage" created by padding.
        public static CryptoStream Decryptor (byte[] key, Stream stream, int keySize)
        {
            var aes = new AesManaged()
            {
                BlockSize = 128,
                KeySize = keySize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            byte[] bytes = new byte[aes.BlockSize / 8];
            stream.Read(bytes, 0, bytes.Length);
            aes.IV = bytes;

            aes.Key = key;

            return new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        }

        /// <summary>
        /// Generate Key: Form key from two byte sources (key source data and salt data), using PBKDF.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateKey (byte[] keyData, byte[] saltData, int sizeBytes)
        {
            var keyGenerator = new Rfc2898DeriveBytes(keyData, saltData, 1000);
            return keyGenerator.GetBytes(sizeBytes);
        }
    }
}
