namespace Ekstazz.Saves.Converters
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    internal class SaveConverter : ISaveConverter
    {
        private readonly List<IncrementalSaveConverter> converters = new List<IncrementalSaveConverter>();
        
        public void AddConverter(IncrementalSaveConverter converter)
        {
            converters.Add(converter);
        }
        
        public JObject ConvertToLatest(JObject json)
        {
            if (json == null)
            {
                return null;
            }

            foreach (var converter in converters.OrderBy(c => c.GoalVersion))
            {
                if (converter.CanConvert(json))
                {
                    Debug.Log($"Using <color=yellow>{converter}</color> to convert save into <color=green>{converter.GoalVersion}</color> version");
                    converter.Convert(json);
                }
            }

            return json;
        }
    }
}