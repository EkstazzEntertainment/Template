namespace Ekstazz.Saves.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    
    internal class SaveModel
    {
        public SerializationHeader Header { get; }
        public List<ISaveComponent> Components { get; private set; }
        public bool CameFromNewBuild => Header.Version > Serialization.CurrentVersion;
        public DateTime TimeStamp => Header.SaveTimestamp;
        public SaveMeta Meta { get; set; }

        
        public T Get<T>() where T : ISaveComponent
        {
            return Components.OfType<T>().First();
        }

        public SaveModel(SerializationHeader header, List<ISaveComponent> components)
        {
            Header = header;
            Components = components;
        }
    }
}