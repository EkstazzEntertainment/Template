namespace Ekstazz.Saves
{
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(SerializationVersion), menuName = "Ekstazz/Saves/Serialization Version")]
    public class SerializationVersion : ScriptableObject
    {
        public int currentVersion = 8;
        
        public int minimumVersion = 8;
    }
}