namespace Ekstazz.Saves.Packers
{
    using System;
    using System.IO;
    using System.Text;
    using Ekstazz.Saves.Encryption;
    using Ekstazz.Saves.Packers;
    using Newtonsoft.Json.Bson;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    internal class SavePacker : ISavePacker
    {
        private static readonly int encKeySize = 128;

        private static readonly byte[] Sgs = {0x4F, 0xFB, 0x01, 0x89, 0x57, 0x3C, 0xEF, 0x31};

        private static readonly byte[] KeyData =
        {
            0x83, 0xAF, 0x2D, 0xB2, 0x12, 0xED, 0xD4, 0x82,
            0x74, 0xD1, 0xA5, 0xEB, 0x73, 0x5E, 0x8, 0x7C,
            0x33, 0xFB, 0xC3, 0x6A, 0xE1, 0x78, 0x28, 0x15,
            0x98, 0x65, 0x89, 0x83, 0x72, 0x9F, 0xd, 0xA
        };

        private readonly byte[] key;

        public SavePacker()
        {
            key = EncryptionHelper.GenerateKey(KeyData, Sgs, encKeySize / 8);
        }

        public JObject Unpack(byte[] raw)
        {
            if (raw == null)
            {
                return null;
            }

            //try decrypt
            try
            {
                using (var ms = new MemoryStream(raw))
                using (var cStream = EncryptionHelper.Decryptor(key, ms, encKeySize))
                    // using (var lz4Stream = new LZ4Stream(cStream, LZ4StreamMode.Decompress))
                using (var br = new BsonReader(cStream, false, DateTimeKind.Utc))
                {
                    return JObject.Load(br);
                }
            }
            catch (Exception)
            {
                Debug.LogWarning($"Can not decrypt save. May be, raw data is a text string");
            }

            //or parse directly
            var jsonString = Encoding.Unicode.GetString(raw);
            return JObject.Parse(jsonString);
        }

        public byte[] Pack(JObject json)
        {
            using (var ms = new MemoryStream())
            using (var cs = EncryptionHelper.Encryptor(key, ms, encKeySize))
                //    using (var ls = new LZ4Stream(cs, LZ4StreamMode.Compress))
            using (var bs = new BsonWriter(cs))
            {
                bs.DateTimeKindHandling = DateTimeKind.Utc;
                json.WriteTo(bs);
                bs.Flush();
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
}