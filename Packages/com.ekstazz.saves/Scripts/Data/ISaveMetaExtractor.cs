namespace Ekstazz.Saves.Data
{
    internal interface ISaveMetaExtractor
    {
        SaveMeta GetMeta(SaveModel data);
    }

    internal abstract class SaveMeta
    {
        public abstract bool IsEqualTo(SaveMeta meta);
        public abstract CompareResult CompareTo(SaveMeta meta);
    }

    internal class CompareResult
    {
        public bool AreEqual { get; set; }
        public SaveMeta BestOne { get; set; }
    }
    
    internal abstract class SaveMeta<T> : SaveMeta where T : SaveMeta<T>
    {
        public override bool IsEqualTo(SaveMeta meta)
        {
            if (meta is T typedMeta)
            {
                return IsEqualToInternal(typedMeta);
            }

            return false;
        }

        protected abstract bool IsEqualToInternal(T typedMeta);

        public override CompareResult CompareTo(SaveMeta meta)
        {
            if (meta is T typedMeta)
            {
                return CompareToInternal(typedMeta);
            }

            return null;
        }
        
        protected abstract CompareResult CompareToInternal(T typedMeta);
    }
}