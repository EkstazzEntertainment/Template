namespace Ekstazz.Saves.Converters
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    
    internal class SaveFromNewVersionConverter : IncrementalSaveConverter
    {
        protected override bool CanConvertFromVersion(int version)
        {
            return version > Serialization.CurrentVersion;
        }

        public override int GoalVersion => Serialization.CurrentVersion + 1;

        protected override void ConvertInternal(JObject jObject)
        {
            var properties = jObject.Properties().ToList();
            foreach (var property in properties)
            {
                if (property.Name != "header")
                {
                    jObject.Remove(property.Name);
                }
            }
        }
    }
}