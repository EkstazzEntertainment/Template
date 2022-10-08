namespace Ekstazz.Saves.Data
{
    internal class RemoteSaveData : SaveData
    {
        public RemoteSaveData(SaveParsingResult result) : base(result)
        {
        }

        public RemoteSaveData(SaveModel model) : base(model) { }
    }
}