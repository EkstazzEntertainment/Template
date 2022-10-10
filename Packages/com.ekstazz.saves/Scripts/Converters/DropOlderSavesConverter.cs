namespace Ekstazz.Saves.Converters
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    
    internal class DropOlderSavesConverter : IncrementalSaveConverter
    {
        private readonly int minVersionNotToDrop;

        
        public DropOlderSavesConverter(int minVersionNotToDrop)
        {
            this.minVersionNotToDrop = minVersionNotToDrop;
        }

        protected override bool CanConvertFromVersion(int version)
        {
            return version < minVersionNotToDrop;
        }

        public override int GoalVersion => minVersionNotToDrop;
        
        protected override void ConvertInternal(JObject jObject)
        {
            var properties = jObject.Properties().ToList();
            foreach (var property in properties)
            {
                jObject.Remove(property.Name);
            }
        }
    }
}