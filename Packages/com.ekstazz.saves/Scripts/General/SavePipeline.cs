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

    /// <summary>
    /// Takes care about all steps that need to be done
    /// to load, unpack, parse save
    /// </summary>
    internal class SavePipeline
    {
        [Inject]
        internal ISavePacker SavePacker { get; set; }

        [Inject]
        internal ISaveConverter SaveConverter { get; set; }

        [Inject]
        internal ISaveParser SaveParser { get; set; }

        /// <summary>
        /// Contains full pipeline of getting SaveModel from downloading\reading it to converting and parsing
        /// </summary>
        public async Task<SaveParsingResult> GetSaveModelAsync(ISaveIoReader reader, string cid = null)
        {
            try
            {
                //read bytes from source
                var bytes = await reader.Read(cid);

                if (bytes == null)
                {
                    return SaveParsingResult.Empty;
                }

                //convert raw bytes to json
                var json = SavePacker.Unpack(bytes);

                //convert any version json into latest version json
                var latestJson = SaveConverter.ConvertToLatest(json);

                //parse it into data model
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