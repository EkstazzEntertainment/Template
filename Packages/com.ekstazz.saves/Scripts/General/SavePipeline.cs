namespace Ekstazz.Saves
{
    using System;
    using System.Threading.Tasks;
    using Converters;
    using Data;
    using Packers;
    using Parsers;
    using Zenject;
    using Debug = UnityEngine.Debug;

    
    internal class SavePipeline
    {
        [Inject] internal ISavePacker SavePacker { get; set; }
        [Inject] internal ISaveConverter SaveConverter { get; set; }
        [Inject] internal ISaveParser SaveParser { get; set; }

        
        public async Task<SaveParsingResult> GetSaveModelAsync(ISaveIoReader reader, string cid = null)
        {
            try
            {
                var bytes = await reader.Read(cid);

                if (bytes == null)
                {
                    return SaveParsingResult.Empty;
                }

                var json = SavePacker.Unpack(bytes);
                var latestJson = SaveConverter.ConvertToLatest(json);
                var saveModel = SaveParser.FromJson(latestJson);
                
                return new SaveParsingResult {SaveModel = saveModel};
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while processing save by {reader}: {e}");
                return SaveParsingResult.Corrupted;
            }
        }

        public string ToJson(SaveModel model)
        {
            return SaveParser.ToJson(model).ToString();
        }
    }

    internal struct SaveParsingResult
    {
        public static SaveParsingResult Empty => new SaveParsingResult();
        public static SaveParsingResult Corrupted => new SaveParsingResult(){HasErrors = true};
        public SaveModel SaveModel { get; set; }
        public bool HasErrors { get; set; }
        public bool IsEmpty => SaveModel == null;
    }
}