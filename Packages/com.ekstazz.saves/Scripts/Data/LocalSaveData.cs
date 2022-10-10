namespace Ekstazz.Saves.Data
{
    internal class LocalSaveData : SaveData
    {
        public override bool IsLocalSave => true;

        
        public LocalSaveData(SaveParsingResult result) : base(result)
        {
        }

        public LocalSaveData(SaveModel model) : base(model) { }
    }
}