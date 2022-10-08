namespace Ekstazz.ExtensionsAndHelpers
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    
    public static class ObjectExtension
    {
        public static T DeepCopy<T>(this object objSource)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}